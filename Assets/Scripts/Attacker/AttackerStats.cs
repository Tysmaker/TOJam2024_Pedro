using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerStats : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private float attackRange;
    public float detectionRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float cost;
    [SerializeField] private float coolDown;


    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public int GetHealth()
    {
        return health;
    }

    public float AttackRange()
    {
        return attackRange;
    }

    public void SetHealth(int value)
    {
        health -= value;
    }
}
