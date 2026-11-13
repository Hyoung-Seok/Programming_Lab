using UnityEngine;

public class Achievements : MonoBehaviour, IObserver
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
        
        _player.AddObserver(this);
    }

    private void OnDestroy()
    {
        if(_player != null)
            _player.RemoveObserver(this);
    }

    public void OnNotify(GameObject entity, PlayerEvent e)
    {
        switch (e)
        {
            case PlayerEvent.Destroyed:
                _player = null;
                break;
            
            case PlayerEvent.Moved:
                _moveCount++;
                
                if(_moveCount == goalCount)
                    Debug.Log($"[Interface] : {goalCount}번 움직임 업적 달성");
                break;
            
            default:
                return;
        }
    }
}
