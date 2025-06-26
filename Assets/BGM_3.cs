using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_3 : MonoBehaviour
{
   [SerializeField] SFXData bgmData;
    void Start()
    {
     SoundManager.Instance.PlaySFX(bgmData);   
    }
}
