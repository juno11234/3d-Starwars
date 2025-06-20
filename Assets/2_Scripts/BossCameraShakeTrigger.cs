using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCameraShakeTrigger : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    public static BossCameraShakeTrigger Instance;
    public float power = 3f;

    private void Awake()
    {
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        impulseSource.GenerateImpulse(power);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Shake();
        }
    }
}