using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_BossPulse : MonoBehaviour
{
    public int damage = 20;
    public float speed = 20f;
    public float duration = 8f;
    Middle_BossStateMachine middleBoss;
    float currentTime = 0f;

    void Start()
    {
        Vector3 target = Player.CurrentPlayer.transform.position;
        target.y = transform.position.y;
        Vector3 direction = (target - transform.position).normalized;
        transform.forward = direction;
    }


    void Update()
    {
        transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);

        currentTime += Time.deltaTime;
        if (currentTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.CurrentPlayer.GuardBreak();
            
            CombatEvent e = new CombatEvent();

            e.Damage = damage;
            e.Sender = middleBoss;
            e.Reciever = Player.CurrentPlayer;
            e.Collider = other;
            e.HitPosition = other.ClosestPoint(transform.position);

            CombatSysytem.Instance.AddInGameEvent(e);

            Destroy(gameObject);
        }
    }
}