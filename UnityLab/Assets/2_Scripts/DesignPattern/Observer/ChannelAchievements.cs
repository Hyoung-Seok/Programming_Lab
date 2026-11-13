using UnityEngine;

public class ChannelAchievements : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private PlayerEventChannel moveEvent;
    [SerializeField] private PlayerEventChannel destroyEvent;
    
    [SerializeField] private int goalCount = 10;
    
    private int _moveCount = 0;
    
    private void OnEnable()
    {
        moveEvent.OnRaise += OnPlayerMoveEvent;
        destroyEvent.OnRaise += OnPlayerDestroyEvent;
    }

    private void OnDisable()
    {
        moveEvent.OnRaise -= OnPlayerMoveEvent;
        destroyEvent.OnRaise -= OnPlayerDestroyEvent;
    }

    private void OnPlayerMoveEvent()
    {
        _moveCount++;

        if (_moveCount == goalCount)
        {
            Debug.Log($"[SO] : {goalCount}번 움직임 업적 달성");
            moveEvent.OnRaise -= OnPlayerMoveEvent;
        }
    }

    private void OnPlayerDestroyEvent()
    {
        moveEvent.OnRaise -= OnPlayerMoveEvent;
        destroyEvent.OnRaise -= OnPlayerDestroyEvent;
    }
}
