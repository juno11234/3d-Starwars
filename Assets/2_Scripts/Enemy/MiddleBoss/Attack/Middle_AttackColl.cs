using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_AttackColl : MonoBehaviour
{
    private Middle_BossStateMachine boss;
    private int damage = 10;

    private void Awake()
    {
        boss=GetComponentInParent<Middle_BossStateMachine>();
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

            CombatSysytem.Instance.AddInGameEvent(e);
        }
    }
}