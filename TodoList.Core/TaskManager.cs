using Microsoft.Extensions.Logging;
using TodoList.Core.Models;

namespace TodoList.Core;

public class TaskManager : ITaskManager
{
    private readonly ILogger<TaskManager> logger;
    private List<TaskList> taskLists;
    private IStorageManager storageManager;
    public TaskManager(ILoggerFactory loggerFactory, IStorageManager storageManager)
    {
        this.logger = loggerFactory.CreateLogger<TaskManager>();
        this.storageManager = storageManager;
        this.taskLists = storageManager.LoadTaskLists();
    }
}