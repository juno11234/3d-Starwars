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

    [SerializeField] private GameObject warningParticle;
    [SerializeField] private Middle_DashAttackColl dashAttackColl;
    [SerializeField] private GameObject pulse;
    [SerializeField] private GameObject excutionParticle;
    [SerializeField] private GameObject attackColl;

    public BossStat stat;

    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public GameObject WarnigParticle => warningParticle;
    public Middle_DashAttackColl DashAttackColl => dashAttackColl;
    public GameObject ExcutionParticle => excutionParticle;

    private Collider collider;
    private Animator animator;
    private NavMeshAgent agent;
    private float coolTimer;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        stat.hp = stat.maxHp;
        animator = GetComponentInChildren<Animator>();
        warningParticle.SetActive(false);
        excutionParticle.SetActive(false);
        dashAttackColl.gameObject.SetActive(false);
        attackColl.SetActive(false);
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
        if (coolTimer >= stat.skillCool)
        {
            coolTimer = stat.skillCool;
        }
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
        if (Mathf.Approximately(coolTimer, stat.skillCool))
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

            warningParticle.SetActive(true);
            ChangeState(skillState, skillStateType);
        }
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
        stat.hp -= combatEvent.Damage;

        if (stat.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ChangeState(new Middle_ExcutionReadyState(this), MiddleBossStateType.ExcutionReady);
    }
}