using System.Collections.Generic;

public class CommandInvoker
{
    private readonly Stack<ICommand> _commandStack = new(50);
    private readonly Stack<ICommand> _redoStack = new(50);

    public void Execute(ICommand command)
    {
        if (_commandStack.TryPeek(out var lastCommand))
        {
            lastCommand.OnSuperseded();
        }
        command.Execute();
        
        _commandStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (!_commandStack.TryPop(out var lastCommand))
        {
            return;
        }
        
        lastCommand.Undo();
        _redoStack.Push(lastCommand);
    }

    public void Redo()
    {
        if (!_redoStack.TryPop(out var lastCommand))
        {
            return;
        }
        
        lastCommand.Redo();
        _commandStack.Push(lastCommand);
    }
}
