namespace TodoList.Core;

public class CycleDependencyException: Exception 
{
    public CycleDependencyException(string message) : base(message){}
}