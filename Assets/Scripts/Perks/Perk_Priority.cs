using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Priority : MonoBehaviour, IPerkable
{
    [SerializeField] private int priority;
    [SerializeField] private int cost;

    public void Execute()
    {
        var tower = GetComponent<TowerStats>();
        tower.SetPriority(priority);
    }
}
