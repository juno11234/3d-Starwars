using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public BGMData bgm;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }
}