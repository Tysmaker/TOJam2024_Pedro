using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPerksHandler : MonoBehaviour
{
    private List<IPerkable> towerPerks = new List<IPerkable>();


    private void OnEnable()
    {
        if (towerPerks.Count == 0) return;
        var towerStats = GetComponent<TowerStats>();
        foreach (var perk in towerPerks)
        {
            perk.Execute(towerStats);
        }
    }
    public void AddPerk(IPerkable perk)
    {
        towerPerks.Add(perk);
    }
}
