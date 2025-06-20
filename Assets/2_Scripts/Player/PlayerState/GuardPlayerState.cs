using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPlayerState : IPlayerState
{
    private PlayerStateMachine player;
    private float parryTime = 0.5f;
    private float timer = 0f;

    public GuardPlayerState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        timer = 0f;
        player.Animator.SetBool("Guard", true);
        Player.CurrentPlayer.Guard();
        Player.CurrentPlayer.Parry();
    }

    public void Input() { }

    public void UpdateLogic()
    {
        timer += Time.deltaTime;
        
        if (timer >= parryTime) Player.CurrentPlayer.ParryCancel();
        
        var info = player.Animator.GetCurrentAnimatorStateInfo(0);

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