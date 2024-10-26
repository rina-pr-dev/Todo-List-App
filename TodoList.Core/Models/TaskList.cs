namespace TodoList.Core.Models;

public class TaskList
{
    public Guid Id;
    public string Name;
    public List<TodoTask> Tasks;
    public TaskList()
    {
        Tasks = new List<TodoTask>();
    }
}