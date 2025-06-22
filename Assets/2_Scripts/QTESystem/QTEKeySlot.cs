using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEKeySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void MarkSuccess()
    {
        iconImage.color = Color.green;
    }

    public void MarkFailed()
    {
        iconImage.color = Color.red;
    }
}
