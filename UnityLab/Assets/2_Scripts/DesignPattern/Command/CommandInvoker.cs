using System.Collections.Generic;

public class CommandInvoker
{
    private readonly Stack<ICommand> _commandStack = new(50);
    private readonly Stack<ICommand> _redoStack = new(50);

    public void Execute(ICommand command)
    {
        command.Execute();
        
        _commandStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_commandStack.Count == 0)
            return;
        
        var command = _commandStack.Pop();
        command.Undo();
        
        _redoStack.Push(command);
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
            return;
        
        var command = _redoStack.Pop();
        command.Execute();
        
        _commandStack.Push(command);
    }
}
