using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput input;
    private PlayerInput.PlayerActions playerActions;
    private PlayerMovement movement;
    private Vector2 moveInput;
    private Vector2 cameraInput;

    private void Awake()
    {
        input = new PlayerInput();
        playerActions = input.Player;

        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        movement.Move(moveInput);
    }

    private void OnEnable()
    {
        input.Enable();

        playerActions.Move.performed += MoveInput;
        playerActions.Look.performed += CameraInput;

        playerActions.Run.performed += RunInput;
        playerActions.Run.canceled += RunCancel;

        playerActions.Jump.performed += JumpInput;
    }

    private void OnDisable()
    {
        input.Disable();

        playerActions.Move.performed -= MoveInput;
        playerActions.Look.performed -= CameraInput;

        playerActions.Run.performed -= RunInput;
        playerActions.Run.canceled -= RunCancel;

        playerActions.Jump.performed -= JumpInput;
    }

    #region InputSetting

    private void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void CameraInput(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }

    private void RunInput(InputAction.CallbackContext context)
    {
        movement.Run(true);
    }

    private void RunCancel(InputAction.CallbackContext context)
    {
        movement.Run(false);
    }

    private void JumpInput(InputAction.CallbackContext context)
    {
        movement.Jump();
    }

    #endregion
}