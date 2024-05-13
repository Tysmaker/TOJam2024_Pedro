using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITowerButtonInfo : MonoBehaviour
{
    [SerializeField]
    private int towerIndex;
    [SerializeField]
    private TextMeshProUGUI towerName;
    [SerializeField]
    private TextMeshProUGUI towerCost;
    [SerializeField]
    private Slider cooldownSlider;

    private void OnEnable()
    {
        SetButtonInfo();
    }

    private void SetButtonInfo()
    {
        var towerPrefab = PlayerTowersManager.GetTowerPrefab(towerIndex);
        if (towerPrefab == null)
        {
            Debug.LogError("UITowerButtonInfo: towerPrefab is null");
            return;
        }
        var towerStats = towerPrefab.GetComponent<TowerStats>();
        towerName.text = towerStats.GetTowerName();
        towerCost.text = towerStats.GetCost().ToString();
        cooldownSlider.maxValue = towerStats.GetCooldown();
    }

    public void SetCooldownSlider(float value)
    {
        cooldownSlider.value = value;
    }
    public int GetTowerIndex()
    {
        return towerIndex;
    }
}
