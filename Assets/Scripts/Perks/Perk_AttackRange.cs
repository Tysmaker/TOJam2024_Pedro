using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_AttackRange : MonoBehaviour, IPerkable
{
    public string Name => perkName;
    public string Description => description;
    public int Cost => cost;

    [SerializeField] private float attackRange;
    [SerializeField] private int cost;
    [SerializeField] private string perkName;
    [SerializeField] private string description;
    
    public void Execute(TowerStats towerStats)
    {
        towerStats.SetAttackRange(attackRange);
    }
}
