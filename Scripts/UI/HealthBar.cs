using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IDataPersistance
{
    public Slider healthSlider;

    public TMP_Text healthBarText;

    PlayerController player;

    private int playerHp;

    private int playerMaxHp;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnDisable()
    {
        player.PlayerData.HealthChanged -= OnPlayerHealthChanged;
    }

    private float CaculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int newMaxHealth)
    {
        healthSlider.value = CaculateSliderPercentage(newHealth, newMaxHealth);
        healthBarText.text = newHealth + " / " + newMaxHealth;
    }

    public void LoadData(GameData data) // 나중에 고쳐야 되는 부분.. 이렇게 밖에 코드를 짤 수 없는 것인가?
    {
        if (!data.IsNewGame)
        {
            PlayerData playerData = new PlayerData();
            playerData = player.PlayerData.InitialSetStat(1);
            playerHp = playerData.Hp;
            playerMaxHp = playerData.Maxhp;
        }
        else
        {
            playerHp = data.PlayerData.Hp;
            playerMaxHp = data.PlayerData.Maxhp;
        }
        player.PlayerData.HealthChanged += OnPlayerHealthChanged;
        healthSlider.value = CaculateSliderPercentage(playerHp, playerMaxHp);
        healthBarText.text = playerHp + " / " + playerMaxHp;
    }

    public void SaveData(ref GameData data)
    {

    }
}
