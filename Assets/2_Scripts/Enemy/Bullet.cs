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

    private void OnEnable()
    {
        Vector3 target = Player.CurrentPlayer.transform.position + Vector3.up * 1.5f;
        Vector3 direction = (target - transform.position).normalized;
        transform.forward = direction;
    }

    private void Update()
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
            Debug.Log("총맞음");
            Destroy(gameObject);
        }
    }
}