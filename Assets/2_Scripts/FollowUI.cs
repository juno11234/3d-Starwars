using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowUI : MonoBehaviour
{
    public Transform target; // 몬스터 머리 위
    public Vector3 offset = new Vector3(0, 2f, 0); // 머리 위 오프셋
    public Slider hpSlider;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 worldPosition = target.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // UI를 스크린 좌표에 위치시킴
        transform.position = screenPosition;
    }

    public void UpdateHP(float current, float max)
    {
        hpSlider.value = Mathf.Clamp(current / max, 0f, 1f);
    }
}