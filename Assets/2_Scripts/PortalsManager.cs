using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsManager : MonoBehaviour
{
    [SerializeField] Portal[] portals;
    private int index = 0;

    private void Start()
    {
        for (int i = 1; i < portals.Length; i++)
        {
            portals[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (portals[index].IsActivated() && index + 1 < portals.Length)
        {
            portals[index + 1].gameObject.SetActive(true);
            index++;
        }
    }
}