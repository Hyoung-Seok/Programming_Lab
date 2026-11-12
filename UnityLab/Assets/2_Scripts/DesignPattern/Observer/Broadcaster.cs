using System.Collections.Generic;
using UnityEngine;

public class Broadcaster
{
    private HashSet<IObserver> _observers = new HashSet<IObserver>();
    private List<IObserver> _pendingRemove = new();

    public void AddObserver(IObserver observer)
    {
        if (!_observers.Add(observer))
        {
            Debug.LogError($"{observer} is already in the Broadcaster");
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        _pendingRemove.Add(observer);
    }

    public void Notify(GameObject entity, PlayerEvent e)
    {
        ClearPendingRemove();
        
        foreach (var observer in _observers)
        {
            observer.Notify(entity, e);
        }
    }
    
    private void ClearPendingRemove()
    {
        foreach (var observer in _pendingRemove)
        {
            _observers.Remove(observer);
        }
        _pendingRemove.Clear();
    }
}
