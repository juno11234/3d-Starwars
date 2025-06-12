using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [HideInInspector] public Collider collider;

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
            Debug.Log(other.gameObject.name + " HIT");
        }
    }
}