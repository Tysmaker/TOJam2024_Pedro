using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAttackerPlaceButton : MonoBehaviour
{
    private AttackerStats attackerStats;
    private float spawnCoolDown;
    private int unitAmountBeforeCoolDown;

    // UI Elements
    [SerializeField]
    private Slider cooldownSlider;
    [SerializeField]
    private Button placeButton;

    public void StartCooldown()
    {
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        placeButton.interactable = false;
        float cooldown = spawnCoolDown;
        StartCooldownSlider(cooldown);
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        placeButton.interactable = true;
    }

    public void SetAttackerStats(AttackerStats attackerStats)
    {
        this.attackerStats = attackerStats;
        SetSpawnCoolDown();
    }

    public void SetSpawnCoolDown()
    {
        spawnCoolDown = attackerStats.GetSpawnCooldown();
        cooldownSlider.maxValue = spawnCoolDown;
    }

    public void StartCooldownSlider(float value)
    {
        cooldownSlider.maxValue = value;
        cooldownSlider.DOValue(0, value).SetEase(Ease.Linear);
    }
}
