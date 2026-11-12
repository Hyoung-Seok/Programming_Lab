using UnityEngine;

public interface IObserver
{
    public void Notify(GameObject entity, PlayerEvent e);
}
