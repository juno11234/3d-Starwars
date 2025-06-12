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
    Sliding,
    Flying,
    
    Attack,
    Guard,
}

[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    //상태전환과 상태별 로직
    public PlayerStateType CurrentStateType { get; private set; }
    private IPlayerState currentState;

    //입력 받아오는 프로퍼티들
    public Vector2 MoveInput { get; set; }
    public bool RunInput { get; set; }
    public bool JumpInput { get; set; }
    public bool AttackInput { get; set; }
    public bool GuardInput { get; set; }
    public bool FlyingInput { get; set; }

    //외부참조용
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    public WallDetector WallDetector => wallDetector;
    public Transform Model => model;
    public Vector3 Velocity => velocity;
    public List<Transform> waypoints;

    [Header("참조")] [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform model;
    [SerializeField] private Animator animator;
    [SerializeField] private WallDetector wallDetector;

    [Header("스탯")] public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public int maxJumpCount = 2;

    [HideInInspector] public int jumpCount = 0;
    private Vector3 velocity;
    private Vector3 currentWallNormal;
    private Vector3 originalControllerCenter;


    private void Awake()
    {
        controller = controller ?? GetComponent<CharacterController>();
        originalControllerCenter = controller.center;
    }

    private void Start()
    {
        ChangeState(new MoveState(this), PlayerStateType.Move);
    }

    private void Update()
    {
        currentState.Input();
        currentState.UpdateLogic();

        if (isRestoringRotation)
        {
            rotationRestoreTimer += Time.deltaTime;
            float t = rotationRestoreTimer / rotationRestoreDuration;
            model.rotation = Quaternion.Slerp(rotationRestoreStart, rotationRestoreEnd, t);
            if (t >= 1f) isRestoringRotation = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        JumpInput = false;
        AttackInput = false;
    }

    public void ChangeState(IPlayerState newState, PlayerStateType newStateType) //상태전환 로직
    {
        currentState?.Exit();
        currentState = newState;
        CurrentStateType = newStateType;
        currentState?.Enter();
    }

    public void MoveCharacter(Vector2 input, float speed) //이동 로직
    {
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

    public bool TryJump() //점프 로직
    {
        if (jumpCount >= maxJumpCount) return false;
        jumpCount++;
        animator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        return true;
    }

    public float wallJumpHorizontalSpeed = 10f;

    public void TryWallJump(Vector3 jumpDir) //벽 점프 로직
    {
        animator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpCount++;
        controller.Move(jumpDir * (wallJumpHorizontalSpeed * Time.deltaTime));

        return;
    }


    public void ResetJumpCount() //점프 횟수 초기화
    {
        jumpCount = 0;
        animator.ResetTrigger("Jump");
    }

    public void StartWallRun(Vector3 wallNormal) // 벽달리기
    {
        currentWallNormal = wallNormal;

        gravity = 0f;
        velocity.y = 0f;

        controller.center = originalControllerCenter + new Vector3(0f, -0.3f, 0f);

        animator.SetTrigger("WallRun");
        animator.SetFloat("Speed", 1f);
    }

    public void StopWallRun_Or_Flying() // 원상복구
    {
        gravity = -9.81f;
        animator.SetFloat("Speed", 0f);

        controller.center = originalControllerCenter;
    }

    //회전 복구용
    private bool isRestoringRotation;
    private Quaternion rotationRestoreStart;
    private Quaternion rotationRestoreEnd;
    private float rotationRestoreTimer;
    private float rotationRestoreDuration = 0.3f;

    public void InitiateRotationRestore(Quaternion target, float duration)
    {
        isRestoringRotation = true;
        rotationRestoreStart = model.rotation;
        rotationRestoreEnd = target;
        rotationRestoreDuration = duration;
        rotationRestoreTimer = 0f;
    }
   
    public void UsePortal(PlayerStateMachine player) //활공상태
    {
        if (FlyingInput == false) return;
        ChangeState(new FlyingState(player, waypoints), PlayerStateType.Flying);

        velocity = Vector3.zero;
        gravity = 0f;
        FlyingInput = false;
    }
}