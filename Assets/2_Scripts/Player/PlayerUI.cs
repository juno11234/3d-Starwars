using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider staminaSlider;
    public Slider bossHpSlider;
    public Middle_BossStateMachine boss;
    private bool nullboss = false;
    

    void Start()
    {
        hpSlider.maxValue = Player.CurrentPlayer.stats.maxHp;
        staminaSlider.maxValue = Player.CurrentPlayer.stats.maxGuardStamina;
        bossHpSlider.gameObject.SetActive(false);

        if (boss == null)
        {
            
            nullboss = true;
            return;
        }

        bossHpSlider.maxValue = boss.stat.maxHp;
    }


    void Update()
    {
        hpSlider.value = Player.CurrentPlayer.stats.hp;
        staminaSlider.value = Player.CurrentPlayer.stats.guardStamina;

        if (nullboss) return;
        bossHpSlider.value = boss.stat.hp;
    }

    public void BossHpActive()
    {
        bossHpSlider.gameObject.SetActive(true);
    }
}