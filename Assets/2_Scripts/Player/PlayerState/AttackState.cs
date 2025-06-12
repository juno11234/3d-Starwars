using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine player;
    private AnimatorStateInfo info;

    public AttackState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.SetTrigger("Attack");
    }

    public void Input()
    {
        if (player.AttackInput && info.IsName("Attack3") == false)
        {
            player.Animator.SetTrigger("Attack");
        }
    }

    public void UpdateLogic()
    {
        player.UsePortal(player);
        
        info = player.Animator.GetCurrentAnimatorStateInfo(0);

        if ((info.IsName("Attack1") && info.normalizedTime >= 0.95f)
            || (info.IsName("Attack2") && info.normalizedTime >= 0.95f)
            || (info.IsName("Attack3") && info.normalizedTime >= 0.95f))
        {
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
        }
    }

    public void Exit() { }
}