using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void HideAttack()
    {
        animator.SetTrigger("HideAttack");
    }

    public void Appear()
    {
        animator.SetTrigger("Appear");
    }

    public void Scream()
    {
        animator.SetTrigger("Scream");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}