using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Middle_ChaseState : IBossState
{
    private Middle_BossStateMachine boss;
    private float range = 6.5f;


    public Middle_ChaseState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter() { }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(Player.CurrentPlayer.transform.position, boss.transform.position);
        boss.LookPlayer();
        if (boss.OnDie)
        {
            boss.ChangeState(new Middle_DeadState(boss), MiddleBossStateType.ExcutionReady);
        }
        else if (boss.TrySkill())
        {
            boss.UseSkill();
        }
        else if (distance < range)
        {
            boss.ChangeState(new Middle_NormalAttackState(boss), MiddleBossStateType.NormalAttack);
        }
        else
        {
            boss.Agent.isStopped = false;
            boss.Animator.SetFloat("Speed", 1f);
        }

        boss.Agent.SetDestination(Player.CurrentPlayer.transform.position);
    }

    public void Exit()
    {
        boss.Agent.isStopped = true;
        boss.Animator.SetFloat("Speed", 0f);
    }
}