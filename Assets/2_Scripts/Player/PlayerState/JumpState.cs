using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerStateMachine player;
    private bool jumpCheck = false;
    private float jumpStateDelay = 0.2f;
    private float timer = 0f;

    public JumpState(PlayerStateMachine player) => this.player = player;

    public void Enter()
    {
        player.Animator.ResetTrigger("Ground");
        player.TryJump();
        jumpCheck = true;
    }

    public void Input()
    {
        if (player.JumpInput && player.TryJump())
        {
            Debug.Log("DoubleJump");
        }

        if (player.WallDetector.IsTouchingWall(out Vector3 wallNormal) && player.RunInput)
        {
            player.ChangeState(new WallRunState(player, wallNormal), PlayerStateType.WallRun);
        }
    }

    public void UpdateLogic()
    {
        float speed = player.RunInput ? player.runSpeed : player.walkSpeed;
        player.MoveCharacter(player.MoveInput, speed);
        timer += Time.deltaTime;
        JumpCheck();

        if (player.Controller.isGrounded && jumpCheck == false)
        {
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        timer = 0f;
    }

    private void JumpCheck()
    {
        if (timer >= jumpStateDelay)
        {
            jumpCheck = false;
        }
    }
}