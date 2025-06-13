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
    }
    
    [SerializeField]
    private Blade blade;
    
    public static Player CurrentPlayer;
    public PlayerStat stat;

    private CharacterController controller;

    public Collider MainCollider => controller;
    public GameObject GameObject => gameObject;
    private void Awake()
    {
        stat.hp = stat.maxHp;
        CurrentPlayer = this;
        controller = GetComponent<CharacterController>();
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

    public void TakeDamage(CombatEvent combatEvent) { }
}