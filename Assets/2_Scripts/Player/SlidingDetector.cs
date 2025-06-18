using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDetector : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slide"))
        {
            PlayerStateMachine player;
            player = other.GetComponentInParent<PlayerStateMachine>();
            player.SlidingTrigger = true;
            player.slidingTransform = this.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slide"))
        {
            PlayerStateMachine player;
            player = other.GetComponentInParent<PlayerStateMachine>();
            player.SlidingTrigger = false;
        }
    }
}