using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEventChannel", menuName = "Scriptable Objects/PlayerEventChannel")]
public class PlayerEventChannel : ScriptableObject
{
    public event Action OnRaise;

    public void Raise()
    {
        OnRaise?.Invoke();
    }
}
