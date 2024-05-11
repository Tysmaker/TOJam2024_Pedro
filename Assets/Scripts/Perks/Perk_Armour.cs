using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Armour : MonoBehaviour, IPerkable
{

    [SerializeField] private int armourIncrease;
    [SerializeField] private int cost;
    public void Execute()
    {
        var tower = GetComponent<TowerStats>();
        tower.IncreaseArmour(armourIncrease);
    }
}
