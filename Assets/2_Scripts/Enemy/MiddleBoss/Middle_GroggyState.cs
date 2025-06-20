using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_GroggyState : IBossState
{
    private Middle_BossStateMachine boss;
    private float groggyTime = 4f;
    private float timer = 0f;

    public Middle_GroggyState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetBool("Groggy", true);
        boss.Agent.isStopped = true;
        timer = 0f;
    }

    public void UpdateLogic()
    {
        timer += Time.deltaTime;
        if (timer >= groggyTime)
        {
            boss.Animator.SetBool("Groggy", false);
            boss.ChangeState(new Middle_ChaseState(boss), MiddleBossStateType.Chasing);
        }
    }

    public void Exit() { }
}