using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : IPlayerState
{
    private PlayerStateMachine player;

    public GuardState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.SetBool("Guard", true);
    }

    public void Input() { }

    public void UpdateLogic()
    {
        if (player.GuardInput == false)
        {
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        player.Animator.SetBool("Guard", false);
    }
}