using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IFighter
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Die
    }

    private EnemyState state;

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
    private float _halfViewAngleCosine; // 캐시된 절반 시야각의 코사인 값

    private bool isPatrolDelaying = false;

    private void Awake()
    {
        stats.hp = stats.maxHp;
        collider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _halfViewAngleCosine = Mathf.Cos(stats.viewAngle * 0.5f * Mathf.Deg2Rad);
    }

    private void Start()
    {
        CombatSysytem.Instance.RegisterMonster(this);
        ChangeState(EnemyState.Patrol);
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
            case EnemyState.Chase:
                UpdateChase();
                break;
            case EnemyState.Attack:
                UpdateAttack();
                break;
            case EnemyState.Die:
                UpdateDie();
                break;
        }
    }
    
    private void ChangeState(EnemyState newState)
    {
        if (state == newState) return;
        
        state = newState;
        
        switch (state)
        {
            case EnemyState.Patrol:
                agent.isStopped = false;
                agent.speed = 2f;
                animator.SetFloat(SPEED, 0.5f);
                stats.viewDistance = 10f; // Reset view distance
                if (!isPatrolDelaying)
                {
                    PatrolNextPoint();
                }
                break;
            case EnemyState.Chase:
                agent.isStopped = false;
                agent.speed = 9f;
                animator.SetFloat(SPEED, 1f);
                stats.viewDistance = stats.range;
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                animator.ResetTrigger("Move");
                animator.SetTrigger("Attack");
                break;
            case EnemyState.Die:
                agent.isStopped = true;
                collider.enabled = false;
                animator.SetTrigger("Die");
                CombatSysytem.Instance.RemoveMonster(this);
                break;
        }
    }
    
    private void UpdatePatrol()
    {
        if (WatchPlayer() || stats.hp < stats.maxHp)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
        
        PatrolNextPoint();
    }
    
    private void UpdateChase()
    {
        LookPlayer();
        float distance = Vector3.Distance(Player.CurrentPlayer.transform.position, transform.position);

        // Attack if in range and visible
        if (distance <= stats.viewDistance && WatchPlayer())
        {
            ChangeState(EnemyState.Attack);
        }
        // Go back to patrol if player is lost
        else if (distance > stats.range)
        {
            ChangeState(EnemyState.Patrol);
        }
        else
        {
            // Keep chasing
            if (agent.isStopped == false)
            {
                agent.SetDestination(Player.CurrentPlayer.transform.position);
            }
        }
    }
    
    private void UpdateAttack()
    {
        LookPlayer();
        
        currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Shoot") && currentState.normalizedTime >= 1.0f)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
    
        Vector3 directionToPlayer = Player.CurrentPlayer.transform.position - transform.position;
        if (directionToPlayer.sqrMagnitude > (stats.viewDistance * stats.viewDistance))
        {
            ChangeState(EnemyState.Chase);
        }
    }
    
    private void UpdateDie()
    {
        currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Die") && currentState.normalizedTime > 0.95f)
        {
            Destroy(gameObject);
        }
    }

    private void PatrolNextPoint()
    {
        if (patrolPoints.Length == 0 || isPatrolDelaying) return;

        if (agent.pathPending == false && agent.remainingDistance < 0.2f)
        {
            StartCoroutine(PatrolDelay());
        }
    }

    IEnumerator PatrolDelay()
    {
        isPatrolDelaying = true;
        animator.SetFloat(SPEED, 0f);

        yield return new WaitForSeconds(3f);
        
        isPatrolDelaying = false;

        if (state == EnemyState.Patrol)
        {
            destinationIndex = (destinationIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[destinationIndex].position);
            animator.SetFloat(SPEED, 0.5f);
        }
    }

    private bool WatchPlayer()
    {
        Vector3 direction = (Player.CurrentPlayer.transform.position - transform.position);
        float distance = direction.magnitude;
        direction.Normalize();
        float dotProduct = Vector3.Dot(transform.forward, direction);
        float halfViewAngleCosine = _halfViewAngleCosine;

        if (dotProduct < halfViewAngleCosine || distance > stats.viewDistance) return false;

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
    
    public void Attack()
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
        if (state == EnemyState.Die) return;

        stats.hp -= combatEvent.Damage;

        if (stats.hp <= 0)
        {
            ChangeState(EnemyState.Die);
        }
        else
        {
            animator.SetTrigger("Hit");
            if(state == EnemyState.Patrol)
            {
                ChangeState(EnemyState.Chase);
            }
        }
    }

}