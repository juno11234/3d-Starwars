using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerState : IPlayerState
{
    private PlayerStateMachine player;
    public MovePlayerState(PlayerStateMachine player) => this.player = player;

    public void Enter()
    {
        player.ResetJumpCount();
        player.Animator.SetTrigger("Ground");
    }

    public void Input()
    {
        if (player.AttackInput)
        {
            player.ChangeState(new AttackPlayerState(player), PlayerStateType.Attack);
        }

        if (player.GuardInput && Player.CurrentPlayer.Guard())
        {
            player.ChangeState(new GuardPlayerState(player), PlayerStateType.Guard);
        }

        if (player.JumpInput)
        {
            player.ChangeState(new JumpPlayerState(player), PlayerStateType.Jump);
        }
    }

    public void UpdateLogic()
    {
        player.UseSlide(player);
        player.UsePortal(player);

        float speed = player.RunInput ? player.runSpeed : player.walkSpeed;
        player.MoveCharacter(player.MoveInput, speed);
    }

    public void Exit() { }
}