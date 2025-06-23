using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    QTEManager qteManager;
    [SerializeField] private List<string> qteKeys;
    [SerializeField] private float duration;

    private void Awake()
    {
        qteManager = FindAnyObjectByType<QTEManager>();
    }

    public void StartQTE()
    {
        qteManager.StartQTE(qteKeys, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        qteManager.StartQTE(qteKeys, duration);
    }
}