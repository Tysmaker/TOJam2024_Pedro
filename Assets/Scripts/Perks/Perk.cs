using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Perk", menuName = "Perk", order = 0)]
public class Perk : ScriptableObject
{
    
    [SerializeField] private TowerStats.PerkTypes perkType; 

    [SerializeField] private string perkName;
    [SerializeField] private string description;
    [SerializeField] private int valueToIncrease;
    [SerializeField] private int cost;    

    public void Execute(TowerStats towerStats)
    {
        switch (perkType)
        {
            case TowerStats.PerkTypes.Armour:
                towerStats.IncreaseArmour(valueToIncrease);
                break;
            case TowerStats.PerkTypes.Attack:
                towerStats.IncreaseAttack(valueToIncrease);
                break;
            case TowerStats.PerkTypes.AttackSpeed:
                towerStats.IncreaseAttackSpeed(valueToIncrease);
                break;
            case TowerStats.PerkTypes.Range:
                towerStats.SetAttackRange(valueToIncrease);
                break;
            case TowerStats.PerkTypes.Agro:
                towerStats.SetPriority(valueToIncrease);
                break;
            case TowerStats.PerkTypes.Health:
                towerStats.IncreaseMaxHealth(valueToIncrease);
                break;
            case TowerStats.PerkTypes.Cooldown:
                towerStats.IncreaseAttackSpeed(valueToIncrease);
                break;
        }
        towerStats.SetCost(cost);
    }

    public string GetName()
    {
        return perkName;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetValueToIncrease()
    {
        return valueToIncrease;
    }

    public int GetCost()
    {
        return cost;
    }

    public TowerStats.PerkTypes GetPerkType()
    {
        return perkType;
    }

}
