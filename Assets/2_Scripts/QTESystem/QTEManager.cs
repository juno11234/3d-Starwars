using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    private List<string> qteKey;
    private InputActionMap qteMap;
    private int currentIndex = 0;
    private float timer;
    private bool isActive = false;

    public Action OnQTESuccess;
    public Action OnQTEFailed;

    public void StartQTE(List<string> actionNames, float duration)
    {
        //QTE 시작
        qteMap = inputAction.FindActionMap("QTE");
        qteMap.Enable();

        foreach (var action in qteMap.actions)
        {
            if (actionNames.Contains(action.name))
                action.performed += OnActionPerformed;
        }

        qteKey = actionNames;
        currentIndex = 0;
        timer = duration;
        isActive = true;

        Debug.Log("QTE started");
    }

    void Update()
    {
        if (isActive == false) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            EndQTE(false);
        }
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        //버튼 눌림
        if (isActive == false) return;

        string actionName = context.action.name;

        if (actionName == qteKey[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= qteKey.Count) EndQTE(true);
            else Debug.Log("다음키" + qteKey[currentIndex]);
        }
        else EndQTE(false);
    }

    private void EndQTE(bool success)
    {
        isActive = false;
        qteMap.Disable();

        foreach (var action in qteMap.actions)
        {
            action.performed -= OnActionPerformed;
        }

        if (success)
        {
            OnQTESuccess?.Invoke();
            Debug.Log("QTE finished");
        }
        else
        {
            OnQTEFailed?.Invoke();
            Debug.Log("QTE failed");
        }
    }
}