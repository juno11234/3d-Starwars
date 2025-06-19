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
    ExcutionReady
}

public class MiddleBossStateMachine : MonoBehaviour, IFighter
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

    public BossStat stat;

    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;

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
    }

    void Start()
    {
        CombatSysytem.Instance.RegisterMonster(this);
        ChangeState(new MiddleChasePlayerState(this), MiddleBossStateType.Chasing);
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
        if (coolTimer == stat.skillCool)
        {
            coolTimer = 0f;
            IBossState skillState;
            MiddleBossStateType skillStateType;
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                skillState = new MiddleDashAttackPlayerState(this);
                skillStateType = MiddleBossStateType.DashAttack;
            }
            else
            {
                skillState = new MiddlePulseAttackPlayerState(this);
                skillStateType = MiddleBossStateType.PulseAttack;
            }

            ChangeState(skillState, skillStateType);
        }
    }

    public void LookPlayer()
    {
        Vector3 direction = Player.CurrentPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(CombatEvent combatEvent) { }
}