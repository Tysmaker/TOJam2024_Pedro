using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Assets.Scripts.Utils.TweenUtils;

public class WaveDefenderManager : MonoBehaviour
{
    [SerializeField]
    private int waveNumber = 0;
    [SerializeField]
    private float waveCooldown = 10f;
    [SerializeField]
    private int waveEnemiesLimit = 10;
    private List<AttackerBehaviour> activeEnemies = new List<AttackerBehaviour>();
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
        Delay(1, () =>
        {
            SpawnWave();
        }, -1, LoopType.Restart);
    }

    private void SpawnWave()
    {
        foreach (var enemy in enemyPrefabs)
        {
            SpawnAttacker(enemy);
        }
    }

    private void SpawnAttacker(GameObject attacker)
    {
        if (isGameOver) return;

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

        var attackerInstance = Instantiate(attacker, randomPositionInsideSpawnArea, Quaternion.AngleAxis(180, Vector3.up));
        var attackerStats = attackerInstance.GetComponent<AttackerStats>();
        var spawnCoolDown = attackerStats.GetSpawnCoolDown();
        var attackerBehavior = attackerInstance.GetComponent<AttackerBehaviour>();

        activeEnemies.Add(attackerBehavior);

        attackerBehavior.SetEndZone(targetArea.gameObject);

        Delay(spawnCoolDown, () =>
        {
            SpawnAttacker(attacker);
        });
    }

    private bool IsActiveEnemiesLimitReached()
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy.GetAttackerState() == AttackerBehaviour.AttackerStates.Dead)
            {
                activeEnemies.Remove(enemy);
            }
        }
        return activeEnemies.Count >= waveEnemiesLimit;
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
