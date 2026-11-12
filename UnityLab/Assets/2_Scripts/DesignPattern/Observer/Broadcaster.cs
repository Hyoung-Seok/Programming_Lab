using System.Collections.Generic;
using UnityEngine;

public class Broadcaster
{
    private List<IObserver> _observers = new();

    public void AddObserver(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            Debug.LogError($"{observer} is already in the Broadcaster");
            return;
        }
        
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(GameObject entity, PlayerEvent e)
    {
        for (var i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].OnNotify(entity, e);
        }
    }
}
