using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_DashAttackState : IBossState
{
    private Middle_BossStateMachine boss;
    private float speed = 10f;
    Vector3 direction;
   

    public Middle_DashAttackState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetBool("DashAttack", true);
        direction = boss.transform.forward;
        boss.DashAttackColl.gameObject.SetActive(true);
      
        boss.WarnigParticle.Play(true);
    }

    public void UpdateLogic()
    {
     

        if (boss.OnDie)
        {
            boss.ChangeState(new Middle_DeadState(boss), MiddleBossStateType.ExcutionReady);
        }
        else if (boss.DashAttackColl.CrashWall)
        {
            BossCameraShakeTrigger.Instance.Shake();
            boss.ChangeState(new Middle_GroggyState(boss), MiddleBossStateType.Groggy);
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRotation, 1f);

        boss.transform.Translate(direction * (speed * Time.deltaTime), Space.World);
    }

    public void Exit()
    {
        boss.DashAttackColl.gameObject.SetActive(false);
        boss.Animator.SetBool("DashAttack", false);
       
    }
}