using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    [SerializeField]
    private int MaxHealth = 100;
    private int currentHealth;

    // FXs

    [SerializeField]
    private GameObject takeDamageFX;
    [SerializeField]
    private GameObject healthBelow50FX;
    [SerializeField]
    private GameObject healthBelow25FX;
    [SerializeField]
    private GameObject healthBelow10FX;
    [SerializeField]
    private GameObject deathFX;

    // Events

    public static event System.Action<int> OnHealthChanged;
    public static event System.Action OnDeath;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (currentHealth <= MaxHealth / 2)
            {
                if (currentHealth <= MaxHealth / 4)
                {
                    if (currentHealth <= MaxHealth / 10)
                    {
                        healthBelow10FX.SetActive(true);
                    }
                    else
                    {
                        healthBelow25FX.SetActive(true);
                    }
                }
                else
                {
                    healthBelow50FX.SetActive(true);
                }
            }
            takeDamageFX.SetActive(true);
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public void AddCityHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void RemoveCityHealth(int health)
    {
        currentHealth -= health;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        deathFX.SetActive(true);
        OnDeath?.Invoke();
    }

}
