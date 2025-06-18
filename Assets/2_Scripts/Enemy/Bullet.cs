using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float speed = 20f;
    public float duration = 5f;
    public Enemy enemy;

    float currentTime = 0f;
    private bool parryBullet = false;

    private void OnEnable()
    {
        Vector3 target = Player.CurrentPlayer.transform.position + Vector3.up * 1.5f;
        Vector3 direction = (target - transform.position).normalized;
        transform.forward = direction;
    }

    private void Update()
    {
        if (parryBullet)
        {
            transform.Translate(-transform.forward * (speed * Time.deltaTime), Space.World);
        }
        else
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }

        currentTime += Time.deltaTime;
        if (currentTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Player.CurrentPlayer.IsParrying == false)
        {
            CombatEvent e = new CombatEvent();

            e.Damage = damage;
            e.Sender = enemy;
            e.Reciever = Player.CurrentPlayer;
            e.Collider = other;
            e.HitPosition = other.ClosestPoint(transform.position);

            CombatSysytem.Instance.AddInGameEvent(e);

            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && Player.CurrentPlayer.IsParrying)
        {
            currentTime = 0f;
            parryBullet = true;
            damage = 100;
        }

        if (parryBullet && other.CompareTag("Enemy"))
        {
            var monster = CombatSysytem.Instance.GetMonsterOrNull(other);
            CombatEvent e = new CombatEvent();
            e.Damage = damage;
            e.HitPosition = other.ClosestPoint(transform.position);
            e.Sender = Player.CurrentPlayer;
            e.Reciever = monster;
            e.Collider = other;

            CombatSysytem.Instance.AddInGameEvent(e);

            Destroy(gameObject);
        }
    }
}