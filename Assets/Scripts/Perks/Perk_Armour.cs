using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Armour : MonoBehaviour, IPerkable
{
    public string Name => perkName;
    public string Description => description;
    public int Cost => cost;

    [SerializeField] private int armourIncrease;
    [SerializeField] private string perkName;
    [SerializeField] private string description;
    [SerializeField] private int cost;
    
    public void Execute(TowerStats towerStats)
    {
        towerStats.IncreaseArmour(armourIncrease);
        towerStats.SetCost(cost);
    }
}
