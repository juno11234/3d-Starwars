using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IFighter
{
    //플레이어 본체 
    [System.Serializable]
    public class PlayerStat
    {
        public int hp = 100;
        public int maxHp = 100;
        public float guardCool = 2f;
    }

    [SerializeField] private Blade blade;
    [SerializeField] private ParticleSystem parryingParticle;

    public static Player CurrentPlayer;
    public PlayerStat stats;

    private CharacterController controller;
    private Animator animator;

    public Collider MainCollider => controller;
    public GameObject GameObject => gameObject;
    public bool OnDie { get; private set; }
    public bool IsGuarding { get; private set; }
    public bool IsParrying { get; private set; }

    private float guardTimer = 0;

    private void Awake()
    {
        stats.hp = stats.maxHp;
        CurrentPlayer = this;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        OnDie = false;
        guardTimer = stats.guardCool;
    }

    private void Update()
    {
        if (guardTimer > stats.guardCool) return;
        guardTimer += Time.deltaTime;
    }

    public void AttackCoroutine()
    {
        StartCoroutine(AttackForwardMovement());
    }

    private IEnumerator AttackForwardMovement()
    {
        float duration = 0.3f; // 이동 지속 시간
        float elapsed = 0f;
        float moveSpeed = 6f; // 앞으로 미는 속도

        while (elapsed < duration)
        {
            controller.Move(transform.forward.normalized * (moveSpeed * Time.deltaTime));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void AttackCollOn()
    {
        blade.collider.enabled = true;
    }

    public void AttackCollOff()
    {
        blade.collider.enabled = false;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        if (IsParrying)
        {
            parryingParticle.Play(true);
            return;
        }

        if (IsGuarding) return;

        stats.hp -= combatEvent.Damage;

        if (stats.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie = true;
        animator.SetTrigger("Die");
    }

    public void Guard()
    {
        if (guardTimer < stats.guardCool) return;
        IsGuarding = true;
    }

    public void GuardBreak()
    {
        guardTimer = 0;
        IsParrying = false;
        IsGuarding = false;
    }

    public void GuardCancel()
    {
        IsGuarding = false;
    }

    public void Parry()
    {
        IsParrying = true;
    }

    public void ParryCancel()
    {
        IsParrying = false;
    }
}