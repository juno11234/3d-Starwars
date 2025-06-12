using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IFighter
{
    [System.Serializable]
    public class EnemyStat
    {
        public int hp = 100;
        public int maxHp = 100;
        public int range;

        public float viewAngle = 120f;
        public float viewDistance = 10f;
    }

    [SerializeField] Transform[] patrolPoints;

    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    public EnemyStat stats = new EnemyStat();

    private Collider collider;
    private Animator animator;
    private NavMeshAgent agent;
    private int destinationIndex = 0;
    private bool isPatrol;
    private bool combatMode;

    private void Awake()
    {
        stats.hp = stats.maxHp;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    private void Update()
    {
        PatrolNextPoint();
    }

    private void Patrol()
    {
        agent.SetDestination(patrolPoints[destinationIndex].position);
        animator.SetFloat("Speed", 0.5f);
        isPatrol = true;
        agent.speed = 2f;
    }

    private void PatrolNextPoint()
    {
        if (combatMode) return;
        
        if (agent.pathPending == false && agent.remainingDistance < 0.2f && isPatrol)
        {
            if (destinationIndex < patrolPoints.Length - 1)
            {
                destinationIndex++;
                StartCoroutine(PatrolDelay());
            }
            else
            {
                destinationIndex = 0;
                StartCoroutine(PatrolDelay());
            }
        }
    }

    IEnumerator PatrolDelay()
    {
        isPatrol = false;

        animator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(3f);

        Patrol();
    }

    private void WhatchPlayer()
    {
        combatMode = true;
    }

    private void Chase()
    {
        animator.SetFloat("Speed", 1f);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Hit");
    }

    private void Die()
    {
        animator.SetTrigger("Death");
    }
}