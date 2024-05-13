using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_AttackSpeed : MonoBehaviour, IPerkable
{
    public string Name => perkName;
    public string Description => description;
    public int Cost => cost;

    [SerializeField] [Tooltip("Percentage that will be reduced from attack speed rate")] private int attackSpeedAmount;
    [SerializeField] private string perkName;
    [SerializeField] private string description;
    [SerializeField] private int cost;

    public void Execute(TowerStats towerStats)
    {
        towerStats.IncreaseAttack(attackSpeedAmount);
    }
}
