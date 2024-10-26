using TodoList.Core.Models;

namespace TodoList.Core;

public static class TodoListExtensions
{
    public static TodoTask AddBlocker(this TodoTask task, TodoTask blocker)
    {
        if (!task.Dependencies.BlockedByTasks.Contains(blocker.Id))
        {
            task.Dependencies.BlockedByTasks.Add(blocker.Id);
        }

        if (!blocker.Dependencies.BlockingTasks.Contains(task.Id))
        {
            blocker.Dependencies.BlockingTasks.Add(task.Id);
        }

        return task;
    }

    public static TodoTask RemoveBlocker(this TodoTask task, TodoTask blocker)
    {
        task.Dependencies.BlockedByTasks.Remove(blocker.Id);
        blocker.Dependencies.BlockingTasks.Remove(task.Id);
        return task;
    }

    public static TodoTask AddChild(this TodoTask task, TodoTask child)
    {
        if (child.Dependencies.ParentTask != null && child.Dependencies.ParentTask != task.Id)
        {
            throw new InsufficientChildrenException("Child task already has a parent.");
        }

        if (!task.Dependencies.ChildrenTasks.Contains(child.Id))
        {
            task.Dependencies.ChildrenTasks.Add(child.Id);
        }

        child.Dependencies.ParentTask = task.Id;
        return task;
    }

    public static TodoTask RemoveChild(this TodoTask task, TodoTask child)
    {
        if (child.Dependencies.ParentTask != task.Id)
        {
            throw new InsufficientChildrenException("Child task has other parent.");
        }

        task.Dependencies.ChildrenTasks.Remove(child.Id);
        child.Dependencies.ParentTask = null;
        return task;
    }

    public static TodoTask AddRelated(this TodoTask task, TodoTask related)
    {
        if (!task.Dependencies.RelatedTasks.Contains(related.Id))
        {
            task.Dependencies.RelatedTasks.Add(related.Id);
        }

        if (!related.Dependencies.RelatedTasks.Contains(task.Id))
        {
            related.Dependencies.RelatedTasks.Add(task.Id);
        }

        return task;
    }

    public static TodoTask RemoveRelated(this TodoTask task, TodoTask related)
    {
        task.Dependencies.RelatedTasks.Remove(related.Id);
        related.Dependencies.RelatedTasks.Remove(task.Id);
        return task;
    }
}