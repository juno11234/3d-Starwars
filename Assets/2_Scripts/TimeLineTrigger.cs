using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TimeLineTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeLine;

    private void OnTriggerEnter(Collider other)
    {
        timeLine.Play();
    }
}