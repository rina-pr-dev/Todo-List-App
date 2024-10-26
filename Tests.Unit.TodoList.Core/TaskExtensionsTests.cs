namespace Tests.Unit.TodoListCore;

using TodoList.Core;
using TodoList.Core.Models;

[TestClass]
public class TaskExtensionsTests
{
    [TestMethod]
    public void AddBlocker_NonExistingBlocker_AddsBlockerToDependencies()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();

        // Act
        var resultTask = task1.AddBlocker(task2);

        // Assert
        Assert.AreEqual(resultTask, task1);
        Assert.AreEqual(1, task2.Dependencies.BlockingTasks.Count);
        Assert.AreEqual(task1.Id, task2.Dependencies.BlockingTasks[0]);
        Assert.AreEqual(1, task1.Dependencies.BlockedByTasks.Count);
        Assert.AreEqual(task2.Id, task1.Dependencies.BlockedByTasks[0]);
    }

    [TestMethod]
    public void AddBlocker_ExistingBlocker_ReusesBlocker()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.Dependencies.BlockedByTasks.Add(task2.Id);
        task2.Dependencies.BlockingTasks.Add(task1.Id);

        // Act
        var resultTask = task1.AddBlocker(task2);

        // Assert
        Assert.AreEqual(resultTask, task1);
        Assert.AreEqual(1, task2.Dependencies.BlockingTasks.Count);
        Assert.AreEqual(task1.Id, task2.Dependencies.BlockingTasks[0]);
        Assert.AreEqual(1, task1.Dependencies.BlockedByTasks.Count);
        Assert.AreEqual(task2.Id, task1.Dependencies.BlockedByTasks[0]);
    }

    [TestMethod]
    public void AddBlocker_OpositeBlockerExists_ThrowsCycleBlockException()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task2.Dependencies.BlockedByTasks.Add(task1.Id);
        task1.Dependencies.BlockingTasks.Add(task2.Id);

        // Act & Assert
        Assert.ThrowsException<CycleDependencyException>(() => task1.AddBlocker(task2));
    }

    [TestMethod]
    public void AddBlocker_BlockingItself_ThrowsCycleBlockException()
    {
        // Arrange
        var task1 = new TodoTask();

        // Act & Assert
        Assert.ThrowsException<CycleDependencyException>(() => task1.AddBlocker(task1));
    }

    [TestMethod]
    public void RemoveBlocker_ExistingBlocker_RemovesBlocker()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.AddBlocker(task2);

        // Act
        var actualTask = task1.RemoveBlocker(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.BlockedByTasks.Count);
        Assert.AreEqual(0, task2.Dependencies.BlockingTasks.Count);
    }

    [TestMethod]
    public void RemoveBlocker_NonExistingBlocker_SkipsRemoval()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();

        // Act
        var actualTask = task1.RemoveBlocker(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.BlockedByTasks.Count);
        Assert.AreEqual(0, task2.Dependencies.BlockingTasks.Count);
    }

    [TestMethod]
    public void AddChild_NonExistingChild_AddsChild()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();

        // Act
        var actualTask = task1.AddChild(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(1, actualTask.Dependencies.ChildrenTasks.Count);
        Assert.AreEqual(task2.Id, actualTask.Dependencies.ChildrenTasks[0]);
        Assert.AreEqual(task1.Id, task2.Dependencies.ParentTask);
    }

    [TestMethod]
    public void AddChild_ExistingChild_ReusesConnection()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.AddChild(task2);

        // Act
        var actualTask = task1.AddChild(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(1, actualTask.Dependencies.ChildrenTasks.Count);
        Assert.AreEqual(task2.Id, actualTask.Dependencies.ChildrenTasks[0]);
        Assert.AreEqual(task1.Id, task2.Dependencies.ParentTask);
    }

    [TestMethod]
    public void AddChild_AlreadyHadOtherParent_Throws()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        var task3 = new TodoTask();
        task3.AddChild(task2);

        // Act && Assert
        Assert.ThrowsException<InsufficientChildrenException>(() => task1.AddChild(task2));
    }

    [TestMethod]
    public void AddChild_AddItselfAsChild_Throws()
    {
        // Arrange
        var task1 = new TodoTask();

        // Act && Assert
        Assert.ThrowsException<InsufficientChildrenException>(() => task1.AddChild(task1));
    }

    [TestMethod]
    public void RemoveChild_ExistingChild_RemovesChild()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.AddChild(task2);

        // Act
        var actualTask = task1.RemoveChild(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.ChildrenTasks.Count);
        Assert.IsNull(task2.Dependencies.ParentTask);
    }

    [TestMethod]
    public void RemoveChild_NonExistingChild_RemovesChild()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();

        // Act
        var actualTask = task1.RemoveChild(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.ChildrenTasks.Count);
    }

    [TestMethod]
    public void RemoveChild_ChildOfOtherTask_RemovesChild()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        var task3 = new TodoTask();
        task3.AddChild(task2);

        // Act
        var actualTask = task1.RemoveChild(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(task3.Id, task2.Dependencies.ParentTask);
    }

    [TestMethod]
    public void AddRelated_NonExistingRelationship_AddsRelationship()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();

        // Act
        var actualTask = task1.AddRelated(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(1, task1.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(1, task2.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(task1.Id, task2.Dependencies.RelatedTasks[0]);
        Assert.AreEqual(task2.Id, task1.Dependencies.RelatedTasks[0]);
    }

    [TestMethod]
    public void AddRelated_ExistingRelationship_ReusesRelationship()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.AddRelated(task2);

        // Act
        var actualTask = task1.AddRelated(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(1, task1.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(1, task2.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(task1.Id, task2.Dependencies.RelatedTasks[0]);
        Assert.AreEqual(task2.Id, task1.Dependencies.RelatedTasks[0]);
    }

    [TestMethod]
    public void RemoveRelated_ExistingRelationship_RemovesRelationship()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        task1.AddRelated(task2);
        
        // Act
        var actualTask = task1.RemoveRelated(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(0, task2.Dependencies.RelatedTasks.Count);
    }

    [TestMethod]
    public void RemoveRelated_NonExistingRelationship_SkipsRemoval()
    {
        // Arrange
        var task1 = new TodoTask();
        var task2 = new TodoTask();
        
        // Act
        var actualTask = task1.RemoveRelated(task2);

        // Assert
        Assert.AreEqual(task1, actualTask);
        Assert.AreEqual(0, task1.Dependencies.RelatedTasks.Count);
        Assert.AreEqual(0, task2.Dependencies.RelatedTasks.Count);
    }

}