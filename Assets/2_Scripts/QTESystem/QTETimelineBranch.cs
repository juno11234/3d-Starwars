using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class QTETimelineBranch : MonoBehaviour
{
    [SerializeField] PlayableDirector failTimeline;
    QTEManager qteManager;

    private void Awake()
    {
        qteManager = FindAnyObjectByType<QTEManager>();
    }

    private void Start()
    {
        qteManager.OnQTEFailed += QTEFail;
    }

    public void QTEFail()
    {
        failTimeline.Play();
    }
}