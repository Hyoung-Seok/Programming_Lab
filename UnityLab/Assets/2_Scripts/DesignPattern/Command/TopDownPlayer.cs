using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour, IMoveable
{
    public Vector3 Position => transform.position;
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")] 
    [SerializeField] private bool debugMode;
    [SerializeField] private GameObject clickPointCheckObj;
    [SerializeField] private GameObject changePointCheckObj;

    private MainActions _actions;
    private CommandInvoker _commandInvoker;
    private Vector3 _targetPos;
    
    // observer
    private Broadcaster _broadcaster;
    public void AddObserver(IObserver observer) => _broadcaster.AddObserver(observer);
    public void RemoveObserver(IObserver observer) => _broadcaster.RemoveObserver(observer);
    
    // observer-delegate
    public event Action<GameObject, PlayerEvent> OnPlayerEvent;
    
    private void Awake()
    {
        _commandInvoker = new CommandInvoker();
        _actions = new MainActions();
        _broadcaster = new Broadcaster();
        
        _targetPos = transform.position;
    }
    
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        _actions.TopDownPlayer.Enable();
        
        _actions.TopDownPlayer.TopdownMove.performed += OnMoveAction;
        _actions.TopDownPlayer.UndoCommand.performed += OnUndoAction;
        _actions.TopDownPlayer.RedoCommand.performed += OnRedoAction;
    }

    private void OnDisable()
    {
        _actions.TopDownPlayer.TopdownMove.performed -= OnMoveAction;
        _actions.TopDownPlayer.UndoCommand.performed -= OnUndoAction;
        _actions.TopDownPlayer.RedoCommand.performed -= OnRedoAction;
        
        _actions.TopDownPlayer.Disable();
    }

    private void OnDestroy()
    {
        _actions.Dispose();
        
        _broadcaster.Notify(gameObject, PlayerEvent.Destroyed);
        OnPlayerEvent?.Invoke(gameObject, PlayerEvent.Destroyed);
    }

    public void MoveTo(Vector3 pos)
    {
        _targetPos = pos;
    }

    private void OnMoveAction(InputAction.CallbackContext ctx)
    {
        var mouse = Mouse.current.position.ReadValue();
        var ray = playerCamera.ScreenPointToRay(mouse);

        if (Physics.Raycast(ray, out var hit, 10000f, groundLayer))
        {
            var move = new MoveCommand(this, hit.point);
            _commandInvoker.Execute(move);
            
            _broadcaster.Notify(gameObject, PlayerEvent.Moved);
            OnPlayerEvent?.Invoke(gameObject, PlayerEvent.Moved);
            
            if(debugMode)
                CreateCheckPointObject(hit.point, Position);
        }
    }

    private void OnUndoAction(InputAction.CallbackContext ctx)
    {
        _commandInvoker.Undo();
    }

    private void OnRedoAction(InputAction.CallbackContext ctx)
    {
        _commandInvoker.Redo();
    }

    private void CreateCheckPointObject(Vector3 cp1, Vector3 cp2)
    {
        var clickPos = new  Vector3(cp1.x, 0f, cp1.z);
        var charPos = new Vector3(cp2.x, 0f, cp2.z);
        
        Instantiate(clickPointCheckObj, clickPos, Quaternion.identity);
        Instantiate(changePointCheckObj, charPos, Quaternion.identity);
    }
}
