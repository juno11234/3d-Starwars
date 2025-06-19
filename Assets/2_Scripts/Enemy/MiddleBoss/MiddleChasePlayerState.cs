using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiddleChasePlayerState : IBossState
{
    private MiddleBossStateMachine boss;
    private float range = 4f;

    public MiddleChasePlayerState(MiddleBossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetFloat("Speed", 1f);
    }

    public void UpdateLogic()
    {
        boss.LookPlayer();
        boss.UseSkill();
        float distance = Vector3.Distance(Player.CurrentPlayer.transform.position, boss.transform.position);

        if (distance < range)
        {
            boss.ChangeState(new MiddleNormalAttackPlayerState(boss), MiddleBossStateType.NormalAttack);
        }

        boss.Agent.SetDestination(Player.CurrentPlayer.transform.position);
    }

    public void Exit()
    {
        boss.Animator.SetFloat("Speed", 0f);
    }
}