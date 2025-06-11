using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    //이동과 카메라 애니메이션 무기콜라이더 켜기
    private static readonly int SPEED = Animator.StringToHash("Speed");
    private static readonly int JUMP = Animator.StringToHash("Jump");
    private static readonly int GROUND = Animator.StringToHash("Ground");
    private static readonly int ATTACK = Animator.StringToHash("Attack");
    private static readonly int GUARD = Animator.StringToHash("Guard");

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform model;
    [SerializeField] private Animator animator;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private float speed;
    private bool isGrounded;
    private bool isRunning;
    private Vector3 velocity;
    private int jumpCount = 0;

    private bool isAttacking = false;
    private bool isGuarding = false;

    private void Start()
    {
        speed = walkSpeed;
    }

    public void Move(Vector2 input)
    {
        if (isGuarding || AttackCheck()) return;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDir = camForward.normalized * input.y
                          + camRight.normalized * input.x;

        if (moveDir.magnitude >= 0.1f)
        {
            controller.Move(moveDir.normalized * (speed * Time.deltaTime));
            if (isRunning == false)
            {
                animator.SetFloat(SPEED, 0.5f);
            }
            else
            {
                animator.SetFloat(SPEED, 1f);
            }

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            model.rotation = Quaternion.Slerp(model.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
        else animator.SetFloat(SPEED, 0f);

        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
            jumpCount = 0;
            animator.SetBool(GROUND, true);
        }

        controller.Move(velocity * Time.deltaTime);
        isGrounded = controller.isGrounded;
    }

    public void Run(bool running)
    {
        if (running)
        {
            isRunning = true;
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
            isRunning = false;
        }
    }

    public void Jump()
    {
        if (isGuarding || AttackCheck()) return;

        if (isGrounded || jumpCount <= 1)
        {
            animator.SetBool(GROUND, false);
            animator.ResetTrigger(JUMP);
            animator.SetTrigger(JUMP);
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
            jumpCount++;
        }
    }

    public void Attack()
    {
        if (isGrounded == false || isGuarding) return;

        animator.SetTrigger(ATTACK);
    }

    public void AttackCoroutine()
    {
        StartCoroutine(AttackForwardMovement());
    }

    private IEnumerator AttackForwardMovement()
    {
        float duration = 0.3f; // 이동 지속 시간
        float elapsed = 0f;
        float moveSpeed = 5f; // 앞으로 미는 속도

        while (elapsed < duration)
        {
            controller.Move(model.forward.normalized * (moveSpeed * Time.deltaTime));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private bool AttackCheck()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack1")
            || stateInfo.IsName("Attack2")
            || stateInfo.IsName("Attack3"))
        {
            return true;
        }
        else return false;
    }

    public void Guard(bool guarding)
    {
        if (isGrounded == false || AttackCheck()) return;

        isGuarding = guarding;
        animator.SetBool(GUARD, guarding);
    }
}