using System.Collections.Generic;

public class CommandInvoker
{
    private readonly Stack<ICommand> _commandStack = new(50);

    public void Execute(ICommand command)
    {
        command.Execute();
        _commandStack.Push(command);
    }

    public void Undo()
    {
        if (_commandStack.Count == 0)
            return;
        
        _commandStack.Pop().Undo();
    }
}
