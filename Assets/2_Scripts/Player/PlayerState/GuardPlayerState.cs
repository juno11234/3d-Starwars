using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPlayerState : IPlayerState
{
    private PlayerStateMachine player;


    public GuardPlayerState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.SetBool("Guard", true);
        Player.CurrentPlayer.Guard();
        
    }

    public void Input() { }

    public void UpdateLogic()
    {
        var info = player.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Parry") )
        {
            Player.CurrentPlayer.Parry();
        }
        else
        {
            Player.CurrentPlayer.ParryCancel();
        }

        if (player.GuardInput == false)
        {
            player.ChangeState(new MovePlayerState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        player.Animator.SetBool("Guard", false);
        Player.CurrentPlayer.GuardCancel();
        Player.CurrentPlayer.ParryCancel();
    }
}