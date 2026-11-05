using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly IMoveable _unit;
    private readonly Vector3 _targetPos;
    private Vector3 _prevPos;

    public MoveCommand(IMoveable unit, Vector3 targetPos, Vector3 currentPos)
    {
        _unit = unit;
        _targetPos = targetPos;
        _prevPos = currentPos;
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
