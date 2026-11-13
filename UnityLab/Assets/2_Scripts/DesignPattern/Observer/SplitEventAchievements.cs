using UnityEngine;

public class SplitEventAchievements : MonoBehaviour
{
    [SerializeField] private int goalCount = 10;
    
    private TopDownPlayer _player;
    private int _moveCount = 0;
    
    private void Start()
    {
        _player = FindAnyObjectByType<TopDownPlayer>();

        if (_player == null)
        {
            Debug.LogError("Player를 찾지 못했습니다");
            return;
        }

        _player.OnPlayerMove += OnPlayerMoveEvent;
        _player.OnPlayerDestroyed += OnPlayerDestroyEvent;
    }

    private void OnDestroy()
    {
        if (_player == null)
            return;
        
        _player.OnPlayerMove -= OnPlayerMoveEvent;
        _player.OnPlayerDestroyed -= OnPlayerDestroyEvent;
    }

    private void OnPlayerMoveEvent()
    {
        _moveCount++;

        if (_moveCount == goalCount)
        {
            Debug.Log($"[Split] : {goalCount}번 움직임 업적 달성");
            _player.OnPlayerMove -= OnPlayerMoveEvent;
        }
    }

    private void OnPlayerDestroyEvent()
    {
        _player = null;
    }
}
