namespace TodoList.Core;

public class InsufficientChildrenException : Exception
{
    public InsufficientChildrenException(string message) : base(message) { }
}