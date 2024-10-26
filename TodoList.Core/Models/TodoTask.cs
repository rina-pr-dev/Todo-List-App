namespace TodoList.Core.Models;

public class TodoTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
    public TaskDependencies Dependencies { get; set; }

    public TodoTask()
    {
        Id = Guid.NewGuid();
        Dependencies = new();
    }
}