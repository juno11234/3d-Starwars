using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activated = true;
            var player = other.GetComponent<PlayerStateMachine>();
            player.waypoints = waypoints;
            player.FlyingTrigger = true;
        }
    }

    public bool IsActivated()
    {
        return activated;
    }
    
}