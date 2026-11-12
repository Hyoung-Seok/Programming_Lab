using UnityEngine;

public interface IObserver
{
    public void OnNotify(GameObject entity, PlayerEvent e);
}
