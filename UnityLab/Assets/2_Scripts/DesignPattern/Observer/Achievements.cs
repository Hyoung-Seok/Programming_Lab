using UnityEngine;

public class Achievements : MonoBehaviour, IObserver
{
    private TopDownPlayer _player;
    private int _moveCount = 0;
    private int _goalCount = 10;

    private void Start()
    {
        _player = FindAnyObjectByType<TopDownPlayer>();

        if (_player == null)
        {
            Debug.LogError("Player를 찾지 못했습니다");
            return;
        }
        
        _player.AddObserver(this);
    }

    private void OnDestroy()
    {
        if(_player != null)
            _player.RemoveObserver(this);
    }

    public void Notify(GameObject entity, PlayerEvent e)
    {
        switch (e)
        {
            case PlayerEvent.REMOVE:
                _player = null;
                break;
            
            case PlayerEvent.MOVE:
                _moveCount++;
                
                if(_moveCount == _goalCount)
                    Debug.Log("10번 움직임 업적 달성");
                break;
            
            default:
                return;
        }
    }
}
