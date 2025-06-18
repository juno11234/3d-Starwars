using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TimeLineTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeLine;
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeLine.Play();
            collider.enabled = false;
        }
    }
}