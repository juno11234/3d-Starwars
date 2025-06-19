using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleDashAttackColl : MonoBehaviour
{
    public bool CrashWall { get; set; }
    [SerializeField] private int damage = 20;
    private MiddleBossStateMachine boss;


    private void Awake()
    {
        boss = GetComponentInParent<MiddleBossStateMachine>();
        CrashWall = false;
    }

    private void OnEnable()
    {
        CrashWall = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CombatEvent e = new CombatEvent();
            e.Reciever = Player.CurrentPlayer;
            e.Sender = boss;
            e.Damage = damage;
            e.HitPosition = other.ClosestPoint(transform.position);
        }

        if (other.CompareTag("Obstacle"))
        {
            CrashWall = true;
        }
    }
}