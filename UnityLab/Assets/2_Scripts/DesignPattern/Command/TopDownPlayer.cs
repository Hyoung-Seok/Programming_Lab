using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour, IMoveable
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayer;

    private MainActions _actions;
    
    private void Awake()
    {
        _actions = new MainActions();
        _actions.TopDownPlayer.TopdownMove.performed += OnMoveAction;
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
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
            MoveTo(hit.point);
        }
    }
}
