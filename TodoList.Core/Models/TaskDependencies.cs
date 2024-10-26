namespace TodoList.Core.Models;

public class TaskDependencies
{
    public Guid? ParentTask {get; set;}
    public List<Guid> BlockedByTasks { get; set; } = new();
    public List<Guid> BlockingTasks { get; set; } = new();
    public List<Guid> ChildrenTasks { get; set; } = new();
    public List<Guid> RelatedTasks { get; set; } = new();
}