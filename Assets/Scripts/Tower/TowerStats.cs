using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private float range;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int priority;
    [SerializeField] private float cost;
    [SerializeField] private float coolDown;   

    public List<IPerkable> perks = new List<IPerkable>();

    private void OnEnable()
    {
        var scripts = GetComponents<IPerkable>();
        foreach (var script in scripts)
        {
            if (script != null)
            {
                perks.Add(script);
            }
        }
        foreach(var p in perks)
        {
            p.Execute();
        }
    }

    public void SetAttackRange(float range)
    {
        this.range = range;
    }

    public void IncreaseArmour(int armourAmount)
    {
        this.armour = armourAmount;
    }
    public void IncreaseAttack(int attackAmount)
    {
        this.attackDamage = attackAmount;
    }
    public void SetPriority(int priority)
    {
        this.priority = priority;
    }

    private void OnDrawGizmosSelected()
    {
      
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
