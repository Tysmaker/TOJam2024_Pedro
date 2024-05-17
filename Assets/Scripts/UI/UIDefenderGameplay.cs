using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDefenderGameplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerTMP;
    [SerializeField]
    private TextMeshProUGUI playerBaseHealthTMP;
    [SerializeField]
    private Slider playerBaseHealthSlider;
    [SerializeField]
    private BaseBehaviour playerBase;
    private float time = 0;

    private void Start()
    {
        BaseBehaviour.OnHealthChanged += SetPlayerCityHealth;

        time = GameplayManager.Instance.GetInitialGameTime();
        SetTimer(time);

        GameplayManager.Instance.OnGameOver += SetInactive;
        GameplayManager.Instance.OnVictory += SetInactive;


        SetPlayerCityMaxHealth(playerBase.GetMaxHealth());
        SetPlayerCityHealth(playerBase.GetCurrentHealth());
    }

    public void Update()
    {
        UpdateTimer();
    }

    private void SetPlayerCityMaxHealth(int health)
    {
        playerBaseHealthSlider.maxValue = health;
    }
    private void SetPlayerCityHealth(int health)
    {
        playerBaseHealthSlider.value = health;
        playerBaseHealthTMP.text = health.ToString();
    }
    private void SetTimer(float timeInSeconds)
    {
        var minutes = Mathf.FloorToInt(timeInSeconds / 60);
        timerTMP.text = string.Format("{0:00}:{1:00}", minutes, timeInSeconds % 60);
    }

    private void UpdateTimer()
    {
        time -= Time.deltaTime;
        SetTimer(time);
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        GameplayManager.Instance.OnGameOver -= SetInactive;
        GameplayManager.Instance.OnVictory -= SetInactive;
    }
}
