using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleNormalAttackPlayerState : IBossState
{
    private MiddleBossStateMachine boss;

    public MiddleNormalAttackPlayerState(MiddleBossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetTrigger("Attack");
    }

    public void UpdateLogic()
    {
        var info = boss.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Attack2") && info.normalizedTime >= 0.9f)
        {
            boss.ChangeState(new MiddleChasePlayerState(boss), MiddleBossStateType.Chasing);
        }
        boss.LookPlayer();
    }

    public void Exit() { }
}