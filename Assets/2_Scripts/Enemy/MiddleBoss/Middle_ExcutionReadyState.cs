using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_ExcutionReadyState : IBossState
{
    private Middle_BossStateMachine boss;
    private float range;

    public Middle_ExcutionReadyState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetBool("Groggy", true);
        boss.ExcutionParticle.SetActive(true);
    }

    public void UpdateLogic()
    {
        float distance = Vector3.Distance(Player.CurrentPlayer.transform.position, boss.transform.position);
        
    }

    public void Exit() { }
}