using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePulseAttackPlayerState : IBossState
{
    private MiddleBossStateMachine boss;

    public MiddlePulseAttackPlayerState(MiddleBossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetTrigger("JumpAttack");
    }

    public void UpdateLogic()
    {
        var info = boss.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("JumpAttack") && info.normalizedTime >= 0.9f)
        {
            boss.ChangeState(new MiddleChasePlayerState(boss), MiddleBossStateType.Chasing);
        }
    }
    public void Exit() { }
}