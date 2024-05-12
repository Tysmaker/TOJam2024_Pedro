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

    private void Start()
    {
        WaveDefenderManager.Instance.OnPlayerCityHealthChanged += SetPlayerCityHealth;
        

        SetPlayerCityMaxHealth(WaveDefenderManager.Instance.GetPlayerCityHealth());
        
        SetTimer(WaveDefenderManager.Instance.GetInitialGameTime());
    }

    private void SetPlayerCityMaxHealth(float health)
    {
        playerBaseHealthSlider.maxValue = health;
    }
    private void SetPlayerCityHealth(float health)
    {
        playerBaseHealthSlider.value = health;
        playerBaseHealthTMP.text = health.ToString();
    }
    private void SetTimer(float timeInSeconds)
    {
        var minutes = Mathf.FloorToInt(timeInSeconds / 60);
        timerTMP.text = string.Format("{0:00}:{1:00}", minutes, timeInSeconds % 60);
    }
}
