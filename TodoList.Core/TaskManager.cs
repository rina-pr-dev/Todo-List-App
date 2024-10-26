using Microsoft.Extensions.Logging;
using TodoList.Core.Models;

namespace TodoList.Core;

public class TaskManager
{
    private readonly ILogger<TaskManager> logger;

    public TaskManager(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<TaskManager>();
    }

    public TodoTask CreateTodoTask(string title, string description = null)
    {
        var task = new TodoTask(){
            Id = Guid.NewGuid(),
            CreationTime = DateTime.UtcNow,
            Title = title,
            Description = description,
        }
    }

    
}