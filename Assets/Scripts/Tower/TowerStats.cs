using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    public enum PerkTypes
    {
        Armour,
        Attack,
        AttackSpeed,
        Range,
        Agro,
        Health,
        Cooldown
    }

    [SerializeField] private string towerName;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private float range;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int agro;
    [SerializeField] private float cost;
    [SerializeField] private float coolDown;

    public event Action<int> OnHealthChanged;

    public void GetTowerInfo(Dictionary<string, string> towerInfo)
    {
        towerInfo.Add(PerkTypes.Health.ToString(), health.ToString());
        towerInfo.Add(PerkTypes.Range.ToString(), range.ToString());
        towerInfo.Add(PerkTypes.Attack.ToString(), attackDamage.ToString());
        towerInfo.Add(PerkTypes.AttackSpeed.ToString(), attackSpeed.ToString("N2") + " s/atk");
        towerInfo.Add(PerkTypes.Agro.ToString(), agro.ToString());
        towerInfo.Add(PerkTypes.Cooldown.ToString(), coolDown.ToString());
        towerInfo.Add("Cost", cost.ToString());
    }

    public float GetTowerInfo(PerkTypes perkType)
    {
        switch (perkType)
        {
            case PerkTypes.Health:
                return health;
            case PerkTypes.Armour:
                return armour;
            case PerkTypes.Range:
                return range;
            case PerkTypes.Attack:
                return attackDamage;
            case PerkTypes.AttackSpeed:
                return attackSpeed;
            case PerkTypes.Agro:
                return agro;
            case PerkTypes.Cooldown:
                return coolDown;
            default:
                return 0;
        }
    }


    public void SetAttackRange(float range)
    {
        this.range += range;
    }

    public void IncreaseArmour(int armourAmount)
    {
        this.armour += armourAmount;
    }

    public void IncreaseMaxHealth(int value)
    {
        this.maxHealth += value;
    }
    public void IncreaseAttack(int attackAmount)
    {
        this.attackDamage += attackAmount;
    }
    public void IncreaseAttackSpeed(float attackSpeedAmount)
    {
        this.attackSpeed /= attackSpeedAmount;
    }
    public void SetPriority(int priority)
    {
        this.agro += priority;
    }
    public int GetPriority()
    {
        return agro;
    }
    public float GetCost()
    {
        return cost;
    }
    public void SetCost(float cost)
    {
        this.cost += cost;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
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

    public float GetAttackRange()
    {
        return range;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void SetHealth(int value)
    {
        health -= value;
        OnHealthChanged?.Invoke(health);
    }
    public float GetCooldown()
    {
        return coolDown;
    }
    public string GetTowerName()
    {
        return towerName;
    }
}
