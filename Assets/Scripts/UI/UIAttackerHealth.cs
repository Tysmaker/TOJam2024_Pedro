using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Utils.TweenUtils;


public class UIAttackerHealth : MonoBehaviour
{
    
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private AttackerStats attackerStats;

    private void Start()
    {
        healthSlider.maxValue = attackerStats.GetMaxHealth();
        healthSlider.value = attackerStats.GetHealth();
        attackerStats.OnHealthChanged += UpdateHealth;
    }

    private void UpdateHealth(int health)
    {
        healthSlider.value = health;
    }

    public void Update()
    {
        LookTowardsCamera();
    }

    private void LookTowardsCamera()
    {
        LookAt(transform, Camera.main.transform, 0);
    }
}
