using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IFighter
{
    private static readonly int SPEED = Animator.StringToHash("Speed");

    [System.Serializable]
    public class EnemyStat
    {
        public int hp = 100;
        public int maxHp = 100;

        public float viewAngle = 120f;
        public float viewDistance = 10f;
        public float range = 30f;
    }

    [SerializeField] Transform[] patrolPoints;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SFXData fireSound;
    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    public EnemyStat stats = new EnemyStat();

    private Collider collider;
    private Animator animator;
    private NavMeshAgent agent;
    private int destinationIndex = 0;
    private AnimatorStateInfo currentState;

    private bool isPatrol;
    private bool combatMode = false;
    private bool die = false;

    private void Awake()
    {
        stats.hp = stats.maxHp;
        collider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        CombatSysytem.Instance.RegisterMonster(this);
        Patrol();
    }

    private void Update()
    {
        if (die) return;

        if (combatMode == false)
        {
            if (WhatchPlayer() || stats.hp < stats.maxHp) ChaseStart();

            PatrolNextPoint();
        }
        else
        {
            float distance = Vector3.Distance(Player.CurrentPlayer.transform.position, transform.position);
            currentState = animator.GetCurrentAnimatorStateInfo(0);

            LookPlayer();

            if (distance < stats.viewDistance && WhatchPlayer())
            {
                Attack();
            }
            else
            {
                Chase();
            }
        }
    }

    private void Patrol()
    {
        if (combatMode) return;
        if (patrolPoints.Length == 0)
        {
            return;
        }

        agent.SetDestination(patrolPoints[destinationIndex].position);
        animator.SetFloat(SPEED, 0.5f);
        isPatrol = true;
        agent.speed = 2f;
    }

    private void PatrolNextPoint()
    {
        if (patrolPoints.Length == 0) return;

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

        animator.SetFloat(SPEED, 0f);
        yield return new WaitForSeconds(3f);

        Patrol();
    }

    private bool WhatchPlayer()
    {
        Vector3 direction = (Player.CurrentPlayer.transform.position - transform.position);
        float distance = direction.magnitude;
        direction.Normalize();
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > stats.viewAngle * 0.5f || distance > stats.viewDistance) return false;

        Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit, stats.viewDistance);

        if (hit.collider == Player.CurrentPlayer.MainCollider
            || hit.collider.TryGetComponent(out WallDetector wallDetector))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void ChaseStart()
    {
        combatMode = true;
        stats.viewDistance = stats.range;
        animator.SetFloat(SPEED, 1f);
        agent.speed = 9f;
        agent.SetDestination(Player.CurrentPlayer.transform.position);
    }


    private void Chase()
    {
        if (currentState.IsName("Shoot")) return;

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Move");
        agent.isStopped = false;
        animator.SetFloat(SPEED, 1f);
        agent.SetDestination(Player.CurrentPlayer.transform.position);
    }

    private void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(fireSound);
    }

    private void LookPlayer()
    {
        Vector3 direction = Player.CurrentPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        if (die) return;

        stats.hp -= combatEvent.Damage;

        if (stats.hp <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        die = true;
        agent.isStopped = true;
        collider.enabled = false;
        animator.SetTrigger("Die");
    }
}