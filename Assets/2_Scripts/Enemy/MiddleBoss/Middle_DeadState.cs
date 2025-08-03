using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.SceneManagement;

public class Middle_DeadState : IBossState
{
    private Middle_BossStateMachine boss;
    float timer = 0;
    private float delay = 3f;

    public Middle_DeadState(Middle_BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Animator.SetTrigger("Die");
    }

    public void UpdateLogic()
    {
        var info = boss.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Death") && info.normalizedTime > 0.9f)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                SoundManager.Instance.StopAllSFX();
                SceneManager.LoadScene(0);

            }
        }
    }

    public void Exit() { }
}