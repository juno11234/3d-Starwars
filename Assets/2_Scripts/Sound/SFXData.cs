using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXData")]
public class SFXData : ScriptableObject
{
    public string sfxName;
    public AudioClip sfxClip;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
}

[CreateAssetMenu(fileName = "BGMData")]
public class BGMData : ScriptableObject
{
    public string bgmName;
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
}