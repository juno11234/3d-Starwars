using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class QTEKeySprite
{
    public string name;
    public Sprite sprite;
}

public class QTEManager : MonoBehaviour
{
    [SerializeField] private List<QTEKeySprite> keySprites;
    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private Transform keyContainer;
    [SerializeField] private GameObject keySlotPrefab;

    private List<GameObject> keySlots = new List<GameObject>();
    private Dictionary<string, Sprite> keySpriteDict = new Dictionary<string, Sprite>();
    private List<string> qteKey;
    private InputActionMap qteMap;
    private int currentIndex = 0;
    private float timer;
    private bool isActive = false;

    public Action OnQTESuccess;
    public Action OnQTEFailed;

    private void Awake()
    {
        foreach (var entry in keySprites)
            keySpriteDict[entry.name] = entry.sprite;
        
        qtePanel.SetActive(false);
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
        ShowQTEUI();
        Debug.Log("QTE started");
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        //버튼 눌림
        if (isActive == false) return;

        string actionName = context.action.name;

        if (actionName == qteKey[currentIndex])
        {
            MarkKeySuccess(currentIndex);
            currentIndex++;

            if (currentIndex >= qteKey.Count) EndQTE(true);
            else Debug.Log("다음키" + qteKey[currentIndex]);
        }
        else
        {
            MarkKeyFail(currentIndex);
            EndQTE(false);
        }
    }

    private void EndQTE(bool success)
    {
        isActive = false;
        qteMap.Disable();
        HideQTEUI();
        
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

    private void ShowQTEUI()
    {
        
        qtePanel.SetActive(true);
        ClearQTEUI();
        
        foreach (string key in qteKey)
        {
            GameObject slot = Instantiate(keySlotPrefab, keyContainer);

            Image iconImage = slot.GetComponentInChildren<Image>();
            if (keySpriteDict.TryGetValue(key, out Sprite icon))
                iconImage.sprite = icon;
            else
                Debug.LogWarning($"아이콘 없음: {key}");

            keySlots.Add(slot);
        }
    }

    private void MarkKeySuccess(int index)
    {
        if (index < keySlots.Count)
            keySlots[index].GetComponentInChildren<Image>().color = Color.green;
    }

    private void MarkKeyFail(int index)
    {
        if (index < keySlots.Count)
            keySlots[index].GetComponentInChildren<Image>().color = Color.red;
    }

    private void HideQTEUI()
    {
        qtePanel.SetActive(false);
        ClearQTEUI();
    }

    private void ClearQTEUI()
    {
        foreach (var slot in keySlots)
            Destroy(slot);

        keySlots.Clear();
    }
}