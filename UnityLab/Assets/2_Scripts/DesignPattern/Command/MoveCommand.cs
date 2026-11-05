using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly IMoveable _unit;
    private readonly Vector3 _targetPos;
    private Vector3 _prevPos;

    public MoveCommand(IMoveable unit, Vector3 targetPos)
    {
        _unit = unit;
        _targetPos = targetPos;
        _prevPos = _unit.Position;
    }
        
    public void Execute()
    {
        _prevPos = _unit.Position;
        _unit.MoveTo(_targetPos);
    }

    public void Undo()
    {
        _unit.MoveTo(_prevPos);
    }
}
