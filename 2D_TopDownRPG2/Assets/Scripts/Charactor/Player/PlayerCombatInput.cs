using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatInput : BaseMovementInput
{
    private InputActions _inputActions;


    private void Awake()
    {
        _inputActions = InputCentral.InputActions;
    }

    private void Update()
    {
        InputVector = _inputActions.PlayerMove.Move.ReadValue<Vector2>();
    }
}