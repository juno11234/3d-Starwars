using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_DashAttackColl : MonoBehaviour
{
    public bool CrashWall { get; set; }
    [SerializeField] private int damage = 20;
    private Middle_BossStateMachine boss;
    private MiddleBoss_Sound sound;

    private void Awake()
    {
        boss = GetComponentInParent<Middle_BossStateMachine>();
        CrashWall = false;
    }

    private void OnEnable()
    {
        CrashWall = false;
    }

    private void OnDisable()
    {
        CrashWall = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Player.CurrentPlayer.GuardBreak();


            KnockbackClass.KnockbackFunc.Invoke(other.ClosestPoint(transform.position));
            CombatEvent e = new CombatEvent();
            e.Reciever = Player.CurrentPlayer;
            e.Sender = boss;
            e.Damage = damage;
            e.HitPosition = other.ClosestPoint(transform.position);

            CombatSysytem.Instance.AddInGameEvent(e);
        }

        if (other.CompareTag("Obstacle"))
        {
            SoundManager.Instance.PlaySFX(boss.Sound.rushAttack);
            CrashWall = true;
        }
    }
}