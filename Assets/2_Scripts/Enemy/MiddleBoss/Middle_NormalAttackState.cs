using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_NormalAttackState : IBossState
{
    private Middle_BossStateMachine boss;

    public Middle_NormalAttackState(Middle_BossStateMachine boss)
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
            boss.ChangeState(new Middle_ChaseState(boss), MiddleBossStateType.Chasing);
        }
        boss.LookPlayer();
    }

    public void Exit() { }
}