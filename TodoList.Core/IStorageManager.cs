namespace TodoList.Core;

using TodoList.Core.Models;
public interface IStorageManager{
    List<TaskList> LoadTaskLists();
}