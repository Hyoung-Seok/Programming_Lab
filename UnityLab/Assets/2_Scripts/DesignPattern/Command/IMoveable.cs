using UnityEngine;

public interface IMoveable
{
    public Vector3 Position { get; }
    public void MoveTo(Vector3 pos);
}
