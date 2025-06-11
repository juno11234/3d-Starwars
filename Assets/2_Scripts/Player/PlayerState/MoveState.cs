using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    private PlayerStateMachine player;
    public MoveState(PlayerStateMachine player) => this.player = player;

    public void Enter()
    {
        
        player.ResetJumpCount();
        player.Animator.SetTrigger("Ground");
    }

    public void Input()
    {
        if (player.JumpInput)
        {
            player.ChangeState(new JumpState(player), PlayerStateType.Jump);
        }
    }

    public void UpdateLogic()
    {
        player.UsePortal(player);
        
        float speed = player.RunInput ? player.runSpeed : player.walkSpeed;
        player.MoveCharacter(player.MoveInput, speed);
    }

    public void Exit() { }
}