using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSender : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime = 0f;
    private bool passStartNormalizedTime;

    [Range(0f, 1f)] public float endNormalizedTime = 0f;
    private bool passEndNormalizedTime;

    private Middle_BossStateMachine boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponentInParent<Middle_BossStateMachine>();
        passStartNormalizedTime = false;
        passEndNormalizedTime = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (passStartNormalizedTime == false && startNormalizedTime < stateInfo.normalizedTime)
        {
            boss.AttackCollOn();
            passStartNormalizedTime = true;
        }

        if (passEndNormalizedTime == false && endNormalizedTime < stateInfo.normalizedTime)
        {
            boss.AttackCollOff();
            passEndNormalizedTime = true;
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}