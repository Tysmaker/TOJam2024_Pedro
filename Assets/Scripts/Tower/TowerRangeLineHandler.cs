using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeLineHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject towerRangeLine;
    private TowerStats towerStats;

    // Update is called once per frame
    void Update()
    {
        UpdateTowerRangeLine();
    }

    private void UpdateTowerRangeLine()
    {
        if (towerStats == null)
        {
            Debug.LogError("TowerStats is null");
            return;
        }
        float xScale = towerStats.GetAttackRange() / 5;
        float zScale = towerStats.GetAttackRange() / 5;

        towerRangeLine.transform.localScale = new Vector3(xScale, 0, zScale);
    }

    public void SetTowerStats(TowerStats towerStats)
    {
        this.towerStats = towerStats;
    }
}
