using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Attack : MonoBehaviour, IPerkable
{
    public string Name => perkName;
    public string Description => description;
    public int Cost => cost;
    
    [SerializeField] private int attackIncrease;
    [SerializeField] private string perkName;
    [SerializeField] private string description;
    [SerializeField] private int cost;

    public void Execute(TowerStats towerStats)
    {
        towerStats.IncreaseAttack(attackIncrease); 
        towerStats.SetCost(cost);
    }
}
