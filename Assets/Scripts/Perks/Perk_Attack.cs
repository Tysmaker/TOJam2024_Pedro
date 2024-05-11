using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Attack : MonoBehaviour, IPerkable
{
    [SerializeField] private int attackIncrease;
    [SerializeField] private int cost;

    public void Execute()
    {
        var tower = GetComponent<TowerStats>();
        tower.IncreaseAttack(attackIncrease); 
    }
}
