using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : IPlayerState
{
    private PlayerStateMachine player;
    private Vector3 wallNormal;
    private float timer;
    private float duration = 0.2f;

    private float runCool = 1f;
    private float runTimer = 0f;

    public WallJumpState(PlayerStateMachine p, Vector3 wallNormal)
    {
        player = p;
        this.wallNormal = wallNormal;
    }

    public void Enter()
    {
        Debug.Log("벽점프!!");
        Vector3 jumpDir = (wallNormal + Vector3.up).normalized;

        player.TryWallJump(jumpDir);
        timer = 0;
        runTimer = 0;
    }

    public void Input()
    {
        if (player.JumpInput && player.TryJump()) { }

        if (player.WallDetector.IsTouchingWall(out Vector3 wallNormal) && player.RunInput && WallRunCool())
        {
            player.ChangeState(new WallRunState(player, wallNormal), PlayerStateType.WallRun);
        }
    }

    public void UpdateLogic()
    {
        runTimer += Time.deltaTime;

        if (timer < duration)
        {
            Vector3 jumpDir = (wallNormal + Vector3.up).normalized;
            player.Controller.Move(jumpDir * (player.wallJumpHorizontalSpeed * Time.deltaTime));
            timer += Time.deltaTime;
        }

        player.MoveCharacter(player.MoveInput, player.RunInput ? player.runSpeed : player.walkSpeed);

        // 착지 시 MoveState로
        if (player.Controller.isGrounded && timer >= duration)
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
    }

    public void Exit() { }

    public bool WallRunCool()
    {
        return runTimer >= runCool;
    }
}