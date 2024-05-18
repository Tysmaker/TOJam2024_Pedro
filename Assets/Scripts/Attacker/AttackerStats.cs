using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerStats : MonoBehaviour
{
    [SerializeField] private string attackerName;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private float attackRange;
    public float detectionRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private int creditsOnDeath;
    [SerializeField] private int amountSpawnAtOnce;

    public event System.Action<int> OnHealthChanged;

    public string GetName()
    {
        return attackerName;
    }

    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetArmour()
    {
        return armour;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetSpawnCooldown()
    {
        return cooldown;
    }    

    public float GetAttackRange()
    {
        return attackRange;
    }
    
    public void SetHealth(int value)
    {
        health -= value;
        OnHealthChanged?.Invoke(health);
    }

    public int GetReward()
    {
        return creditsOnDeath;
    }

    public int GetAmountSpawnAtOnce()
    {
        return amountSpawnAtOnce;
    }
}
