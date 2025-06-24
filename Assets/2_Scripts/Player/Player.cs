using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public int guardStamina = 50;
        public int maxGuardStamina = 50;
    }

    [SerializeField] private Blade blade;
    [SerializeField] private ParticleSystem parryingParticle;

    public static Player CurrentPlayer;
    public PlayerStat stats;
    public bool excutionInput;
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
        stats.guardStamina = stats.maxGuardStamina;
        CurrentPlayer = this;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        OnDie = false;
        guardTimer = stats.guardCool;
    }

    float timer = 0f;

    private void Update()
    {
        if (guardTimer < stats.guardCool)
        {
            guardTimer += Time.deltaTime;
            return;
        }

        if (stats.guardStamina >= stats.maxGuardStamina) return;

        timer += Time.deltaTime;

        if (timer > 1f)
        {
            timer = 0f;
            stats.guardStamina += 1;
        }
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
        if (OnDie) return;
        if (IsParrying)
        {
            parryingParticle.Play(true);
            return;
        }

        if (IsGuarding)
        {
            stats.guardStamina -= combatEvent.Damage;
            if (stats.guardStamina <= 0)
            {
                GuardBreak();
            }

            return;
        }

        stats.hp -= combatEvent.Damage;

        if (stats.hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDie = true;
        animator.SetTrigger("Die");
        StartCoroutine(DeadCoroutine());
    }

    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(0);
    }

    public bool Guard()
    {
        if (stats.guardStamina <= 0)
        {
            return false;
        }

        IsGuarding = true;
        return true;
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