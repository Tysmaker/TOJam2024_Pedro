using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Utils.TweenUtils;

public class UITowerHealth : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TowerStats towerStats;

    private void Start()
    {
        healthSlider.maxValue = towerStats.GetMaxHealth();
        healthSlider.value = towerStats.GetHealth();
        towerStats.OnHealthChanged += UpdateHealth;
    }

    private void UpdateHealth(int health)
    {
        healthSlider.value = health;
    }
}
