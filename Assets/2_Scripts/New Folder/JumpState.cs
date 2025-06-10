using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerStateMachine player;
    public JumpState(PlayerStateMachine player) => this.player = player;


    public void Enter()
    {
        Debug.Log("JumpEnter");
        player.Animator.ResetTrigger("Ground");
        player.DoJump();
    }

    public void Input()
    {
        if (player.JumpInput)
        {
            player.DoJump();
        }
    }

    public void UpdateLogic()
    {
        float speed = player.RunInput ? player.runSpeed : player.walkSpeed;
        player.MoveCharacter(player.MoveInput, speed);

        if (player.Controller.isGrounded)
        {
            player.Animator.SetTrigger("Ground");
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        Debug.Log("JumpExit");
    }
}