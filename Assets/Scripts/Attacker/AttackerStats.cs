using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerStats : MonoBehaviour
{
    public int health;
    [SerializeField] private int armour;
    public float attackRange;
    public float detectionRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float cost;
    [SerializeField] private float coolDown;
}
