using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleDashAttackPlayerState : IBossState
{
    private MiddleBossStateMachine boss;

    public MiddleDashAttackPlayerState(MiddleBossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetBool("DashAttack", true);
    }

    public void UpdateLogic() { }

    public void Exit()
    {
        boss.Animator.SetBool("DashAttack", false);
    }
}