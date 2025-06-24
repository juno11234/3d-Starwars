using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_PulseAttackState : IBossState
{
    private Middle_BossStateMachine boss;

    public Middle_PulseAttackState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.WarnigParticle.Play(true);
        boss.Animator.SetTrigger("JumpAttack");
    }

    public void UpdateLogic()
    {
        var info = boss.Animator.GetCurrentAnimatorStateInfo(0);
        if (boss.OnDie)
        {
            boss.ChangeState(new Middle_DeadState(boss), MiddleBossStateType.ExcutionReady);
        }
        else if (info.IsName("JumpAttack") && info.normalizedTime >= 0.9f)
        {
            boss.ChangeState(new Middle_ChaseState(boss), MiddleBossStateType.Chasing);
        }

        boss.LookPlayer();
    }

    public void Exit() { }
}