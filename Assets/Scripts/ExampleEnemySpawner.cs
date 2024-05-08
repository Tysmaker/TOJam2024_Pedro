using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Utils.TweenUtils;
using static Assets.Scripts.Utils.InstantiateUtils;
using DG.Tweening;

public class ExampleEnemySpawner : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> enemyPrefabs;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private float spawnRate = 1f;

    private void Start()
    {
       // Delay(spawnRate, SpawnEnemy, -1, LoopType.Restart);
       SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Count);
        var randomSpawnRate = Random.Range(0, spawnRate);

        InstantiatePrefab(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnPointIndex].position, spawnPoints[randomSpawnPointIndex].rotation);

        Delay(spawnRate, SpawnEnemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var spawnPoint in spawnPoints)
        {
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }

}
