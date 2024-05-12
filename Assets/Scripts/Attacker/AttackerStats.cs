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
    [SerializeField] private float coolDown;


    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetArmour()
    {
        return armour;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetSpawnCoolDown()
    {
        return coolDown;
    }    

    public float GetAttackRange()
    {
        return attackRange;
    }

    public void SetHealth(int value)
    {
        health -= value;
    }
}
