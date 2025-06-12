using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput input;
    private PlayerInput.PlayerActions playerActions;
    private PlayerStateMachine stateMachine;
    private Vector2 moveInput;
    private Vector2 cameraInput;

    private void Awake()
    {
        input = new PlayerInput();
        playerActions = input.Player;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stateMachine = GetComponent<PlayerStateMachine>();

        playerActions.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();

        playerActions.Run.performed += _ => stateMachine.RunInput = true;
        playerActions.Run.canceled += _ => stateMachine.RunInput = false;

        playerActions.Jump.performed += _ => stateMachine.JumpInput = true;

        playerActions.Attack.performed += _ => stateMachine.AttackInput = true;
    }

    private void Update()
    {
        stateMachine.MoveInput = moveInput;
    }

    private void OnEnable()
    {
        input.Enable();


        // playerActions.Attack.performed += AttackInput;
        //
        // playerActions.Guard.performed += GuardInput;
        // playerActions.Guard.canceled += GuardCancel;
    }

    private void OnDisable()
    {
        input.Disable();

        // playerActions.Move.performed -= MoveInput;
        //
        // playerActions.Run.performed -= RunInput;
        // playerActions.Run.canceled -= RunCancel;
        //
        // playerActions.Jump.performed -= JumpInput;
        //
        // playerActions.Attack.performed -= AttackInput;
        //
        // playerActions.Guard.performed -= GuardCancel;
        // playerActions.Guard.canceled -= GuardCancel;
    }
}