using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [HideInInspector] public Collider collider;

    [SerializeField] private int damage = 35;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    void Start()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var monster = CombatSysytem.Instance.GetMonsterOrNull(other);
            CombatEvent e = new CombatEvent();
            e.Damage = damage;
            e.HitPosition = other.ClosestPoint(transform.position);
            e.Sender = Player.CurrentPlayer;
            e.Reciever = monster;
            e.Collider = other;

            CombatSysytem.Instance.AddInGameEvent(e);
        }
    }
}