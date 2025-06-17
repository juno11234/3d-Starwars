using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    

    public void Shake()
    {
        impulseSource.GenerateImpulse(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        Shake();
    }
}
