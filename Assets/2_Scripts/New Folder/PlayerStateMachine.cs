using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerStateType
{
    Move,
    Jump,

    WallRun,
    WallJump,

    Attack,
    Guard,

    Sliding,

    Flying,
}

[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerStateType CurrentStateType { get; private set; }
    private IPlayerState currentState;

    public Vector2 MoveInput { get; set; }
    public bool RunInput { get; set; }
    public bool JumpInput { get; set; }
    public bool AttackInput { get; set; }
    public bool GuardInput { get; set; }
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    [Header("참조")] [SerializeField] private CharacterController controller;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform model;
    [SerializeField] private Animator animator;

    [Header("스탯")] public float walkSpeed = 5f;

    public float runSpeed = 9f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public int maxJumpCount = 2;

    [HideInInspector] public int jumpCount = 0;
    private Vector3 velocity;

    private void Awake()
    {
        controller = controller ?? GetComponent<CharacterController>();
    }

    private void Start()
    {
        ChangeState(new MoveState(this), PlayerStateType.Move);
    }

    private void Update()
    {
        currentState.Input();
        currentState.UpdateLogic();
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        JumpInput = false;
        AttackInput = false;
    }

    public void ChangeState(IPlayerState newState, PlayerStateType newStateType)
    {//상태전환 로직
        currentState?.Exit();
        currentState = newState;
        CurrentStateType = newStateType;
        currentState?.Enter();
    }

    public void MoveCharacter(Vector2 input, float speed)
    {//이동 로직
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        Vector3 desire = forward.normalized * input.y + right.normalized * input.x;
        if (desire.magnitude >= 0.1f)
        {
            controller.Move(desire.normalized * (speed * Time.deltaTime));
            animator.SetFloat("Speed", speed == runSpeed ? 1f : 0.5f);
            Quaternion targetRot = Quaternion.LookRotation(desire);
            model.rotation = Quaternion.Slerp(model.rotation, targetRot, 10f * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    public bool TryJump()
    {//점프 로직
        if (jumpCount >= maxJumpCount) return false;
        jumpCount++;
        animator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        return true;
    }

    public void ResetJumpCount()
    {//점프 횟수 초기화
        jumpCount = 0;
        animator.SetTrigger("Ground");
        animator.ResetTrigger("Jump");
    }
}