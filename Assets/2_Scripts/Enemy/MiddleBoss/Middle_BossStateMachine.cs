using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum MiddleBossStateType
{
    Chasing,
    NormalAttack,
    PulseAttack,
    DashAttack,
    Groggy,
    ExcutionReady
}

public class Middle_BossStateMachine : MonoBehaviour, IFighter
{
    public MiddleBossStateType CurrentStateType { get; private set; }
    private IBossState currentState;

    [System.Serializable]
    public class BossStat
    {
        public int hp = 300;
        public int maxHp = 300;
        public float skillCool = 15f;
    }

    [SerializeField] private ParticleSystem warningParticle;
    [SerializeField] private Middle_DashAttackColl dashAttackColl;
    [SerializeField] private GameObject pulse;
    [SerializeField] private GameObject excutionParticle;
    [SerializeField] private GameObject attackColl;

    public BossStat stat;

    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public ParticleSystem WarnigParticle => warningParticle;
    public Middle_DashAttackColl DashAttackColl => dashAttackColl;
    public GameObject ExcutionParticle => excutionParticle;
    public bool OnDie => onDie;

    private Collider collider;
    private Animator animator;
    private NavMeshAgent agent;
    private float coolTimer;
    private bool onDie = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        stat.hp = stat.maxHp;
        animator = GetComponentInChildren<Animator>();
        excutionParticle.SetActive(false);
        dashAttackColl.gameObject.SetActive(false);
        attackColl.SetActive(false);
        onDie = false;
    }

    void Start()
    {
        CombatSysytem.Instance.RegisterMonster(this);
        ChangeState(new Middle_ChaseState(this), MiddleBossStateType.Chasing);
        coolTimer = 0f;
        
    }

    void Update()
    {
        currentState.UpdateLogic();
        coolTimer += Time.deltaTime;
    }

    public void ChangeState(IBossState newPlayerState, MiddleBossStateType newStateType)
    {
        currentState?.Exit();
        currentState = newPlayerState;
        CurrentStateType = newStateType;
        currentState?.Enter();
    }

    public void UseSkill()
    {
        coolTimer = 0f;
        IBossState skillState;
        MiddleBossStateType skillStateType;
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            skillState = new Middle_DashAttackState(this);
            skillStateType = MiddleBossStateType.DashAttack;
        }
        else
        {
            skillState = new Middle_PulseAttackState(this);
            skillStateType = MiddleBossStateType.PulseAttack;
        }

        ChangeState(skillState, skillStateType);
    }

    public bool TrySkill()
    {
        if (coolTimer >= stat.skillCool) return true;
        else return false;
    }

    public void PulsePattern()
    {
        Instantiate(pulse, transform.position, Quaternion.identity);
    }

    public void LookPlayer()
    {
        Vector3 direction = Player.CurrentPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void AttackCollOn()
    {
        attackColl.SetActive(true);
    }

    public void AttackCollOff()
    {
        attackColl.SetActive(false);
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        if (onDie) return;
        stat.hp -= combatEvent.Damage;

        if (stat.hp <= 0)
        {
            onDie = true;
        }
    }

    private void Die() { }
}