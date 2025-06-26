using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerState : IPlayerState
{
    private PlayerStateMachine player;
    private bool jumpCheck = false;
    private float jumpStateDelay = 0.2f;
    private float timer = 0f;

    public JumpPlayerState(PlayerStateMachine player) => this.player = player;

    public void Enter()
    {
        player.Animator.ResetTrigger("Ground");
        player.TryJump();
        jumpCheck = true;
    }

    public void Input()
    {
        if (player.JumpInput && player.TryJump()) { }

        if (player.WallDetector.IsTouchingWall(out Vector3 wallNormal) && player.RunInput)
        {
            player.ChangeState(new WallRunPlayerState(player, wallNormal), PlayerStateType.WallRun);
        }
    }

    public void UpdateLogic()
    {
        player.UseSlide(player);
        player.UsePortal(player);
        
        player.MoveCharacter(player.MoveInput, player.runSpeed);
        timer += Time.deltaTime;
        JumpCheck();

        if (player.Controller.isGrounded && jumpCheck == false)
        {
            player.ChangeState(new MovePlayerState(player), PlayerStateType.Move);
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