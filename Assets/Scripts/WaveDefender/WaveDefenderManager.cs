using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Assets.Scripts.Utils.TweenUtils;
using Unity.VisualScripting;

public class WaveDefenderManager : MonoBehaviour
{
    [SerializeField]
    private int waveNumber = 0;
    [SerializeField]
    private float waveCooldown = 10f;
    [SerializeField]
    private int waveEnemiesLimit = 10;
    private List<AttackerBehaviour> activeEnemies = new List<AttackerBehaviour>();
    private List<GameObject> spawnQueue = new List<GameObject>();
    [SerializeField]
    private Transform spawnArea;
    [SerializeField]
    private Transform targetArea;
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();

    private bool isGameOver = false;

    // Events
    public event System.Action<int> OnWaveNumberChanged;

    // Player info

    private int playerCredits = 100;

    public event System.Action<int> OnPlayerCreditsChanged;

    public static WaveDefenderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(StartWave());
        GameplayManager.Instance.OnGameOver += () =>
        {
            SetGameOver(true);
        };
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(waveCooldown);
        waveNumber++;
        OnWaveNumberChanged?.Invoke(waveNumber);
        SpawnWave();
    }

    private void SpawnWave()
    {
        foreach (var enemy in enemyPrefabs)
        {
            spawnQueue.Add(enemy);
        }

        Delay(0.5f, SpawnAttacker, -1 , LoopType.Restart);        
    }

    private void AddEnemyToQueue(GameObject enemy)
    {
        // If the queue is too big, can you not
        if (spawnQueue.Count > 32)
        {
            return;
        }
        spawnQueue.Add(enemy);
    }


    private void SpawnAttacker()
    {
        if (isGameOver)
        {
            return;
        }

        if (IsActiveEnemiesLimitReached())
        {
            Debug.Log("Limit reached");
            return;
        }

        var randomPositionInsideSpawnArea = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2), // X
                0, // Y
                Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2, spawnArea.position.z + spawnArea.localScale.z / 2) // Z
        );

        var attackerToSpawn = spawnQueue[0];

        var attackerInstance = Instantiate(attackerToSpawn, randomPositionInsideSpawnArea, Quaternion.AngleAxis(180, Vector3.up));
        var attackerStats = attackerInstance.GetComponent<AttackerStats>();
        var spawnCoolDown = attackerStats.GetSpawnCoolDown();
        var attackerBehavior = attackerInstance.GetComponent<AttackerBehaviour>();

        spawnQueue.RemoveAt(0);

        activeEnemies.Add(attackerBehavior);

        attackerBehavior.SetEndZone(targetArea.gameObject);

        attackerBehavior.OnDeath += () =>
        {
            RemoveEnemy(attackerBehavior);
        };
        GameplayManager.Instance.OnGameOver += () =>
        {
            attackerBehavior.Death();
        };

        Delay(spawnCoolDown, () =>
        {
            AddEnemyToQueue(attackerToSpawn);
        });
    }

    private bool IsActiveEnemiesLimitReached()
    {
        return activeEnemies.Count >= waveEnemiesLimit;
    }

    public void RemoveEnemy(AttackerBehaviour enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public void AddCredits(int credits)
    {
        playerCredits += credits;
        OnPlayerCreditsChanged?.Invoke(playerCredits);
    }

    public void RemoveCredits(int credits)
    {
        playerCredits -= credits;
        OnPlayerCreditsChanged?.Invoke(playerCredits);
    }

    // Getters and Setters

    public void SetGameOver(bool value)
    {
        isGameOver = value;
    }

    public bool CanAfford(int credits)
    {
        return playerCredits >= credits;
    }
    public void SetPlayerCredits(int credits)
    {
        playerCredits = credits;
    }
}
