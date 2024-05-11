using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_AttackRange : MonoBehaviour, IPerkable
{

    [SerializeField] private float attackRange;
    [SerializeField] private int cost;
    public void Execute()
    {
        var tower = GetComponent<TowerStats>();
        tower.SetAttackRange(attackRange);
    }
}
