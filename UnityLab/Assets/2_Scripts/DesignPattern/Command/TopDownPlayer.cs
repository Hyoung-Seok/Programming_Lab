using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour, IMoveable
{
    public Vector3 Position => transform.position;
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayer;

    private MainActions _actions;
    private CommandInvoker _commandInvoker;
    
    private void Awake()
    {
        _commandInvoker = new CommandInvoker();
        _actions = new MainActions();
    }

    private void OnEnable()
    {
        _actions.TopDownPlayer.Enable();
        
        _actions.TopDownPlayer.TopdownMove.performed += OnMoveAction;
        _actions.TopDownPlayer.PrevCommand.performed += OnUndoAction;
    }

    private void OnDisable()
    {
        _actions.TopDownPlayer.TopdownMove.performed -= OnMoveAction;
        _actions.TopDownPlayer.PrevCommand.performed -= OnUndoAction;
        
        _actions.TopDownPlayer.Disable();
    }

    public void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnMoveAction(InputAction.CallbackContext ctx)
    {
        var mouse = Mouse.current.position.ReadValue();
        var ray = playerCamera.ScreenPointToRay(mouse);

        if (Physics.Raycast(ray, out var hit, 10000f, groundLayer))
        {
            var move = new MoveCommand(this, hit.point);
            _commandInvoker.Execute(move);
        }
    }

    private void OnUndoAction(InputAction.CallbackContext ctx)
    {
        _commandInvoker.Undo();
    }
}
