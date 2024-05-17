using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Utils.InstantiateUtils;

public static class PlayerTowersManager
{
    public static Dictionary<string, TowerStats> playerTowers {get; private set;} = new Dictionary<string, TowerStats>();
    public static bool IsInitialized { get; private set;} = false;

    public static void Init(Transform anchor)
    {
        IsInitialized = true;
        // Select random towers from Resources to give to player
        List<GameObject> towers = new List<GameObject>(Resources.LoadAll<GameObject>("Towers"));
        if (towers.Count == 0)
        {
            Debug.LogError("No towers found in Resources/Towers folder");
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            var randomIndex = Random.Range(0, towers.Count);
            var tower = InstantiatePrefab(towers[randomIndex], anchor.position);
            var towerBehaviour = tower.GetComponent<TowerBehavior>();
            var towerStats = tower.GetComponent<TowerStats>();

            towerBehaviour.SetPersistent();          
            AddTower(towerStats);

            towers.RemoveAt(randomIndex);
        }
    }
    public static void AddTower(TowerStats tower)
    {
        playerTowers.Add(tower.name, tower);
    }

    public static void RemoveTower(TowerStats tower)
    {
        playerTowers.Remove(tower.name);
    }

    public static void ApplyPerk(Perk perk, TowerStats towerStats)
    {
        perk.Execute(towerStats);
    }

    public static GameObject GetTower(string towerName)
    {
        if (!playerTowers.ContainsKey(towerName)) return null;
        return playerTowers[towerName].gameObject;
    }
    public static GameObject GetTowerPrefab(int index)
    {
        foreach (var tower in playerTowers)
        {
            if (index == 0) return tower.Value.gameObject;
            index--;
        }
        return null;
    }
    public static void DisposeOfTowers()
    {
        foreach (var tower in playerTowers)
        {
            GameObject.Destroy(tower.Value.gameObject);
        }
        playerTowers.Clear();
        IsInitialized = false;
    }
}
