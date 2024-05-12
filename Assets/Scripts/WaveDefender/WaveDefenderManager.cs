using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Utils.TweenUtils;

public class WaveDefenderManager : MonoBehaviour
{
    [SerializeField]
    private int waveNumber = 0;
    [SerializeField]
    private float waveCooldown = 10f;
    [SerializeField]
    private Transform spawnArea;
    [SerializeField]
    private Transform targetArea;
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();

    private bool isWaveActive = false;
    private bool isWaveComplete = false;
    private bool isGameOver = false;

    public event System.Action<int> OnWaveNumberChanged;

    // Player info

    private int playerCredits = 100;
    private int playerCityHealth = 100;

    public event System.Action<int> OnPlayerCreditsChanged;
    public event System.Action<int> OnPlayerCityHealthChanged;

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
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(waveCooldown);
        isWaveActive = true;
        isWaveComplete = false;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(waveNumber);
        foreach (var enemy in enemyPrefabs)
        {
            var randomPositionInsideSpawnArea = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2), // X
                0, // Y
                Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2, spawnArea.position.z + spawnArea.localScale.z / 2) // Z
            );
            SpawnAttacker(enemy, randomPositionInsideSpawnArea);
        }
    }

    private void SpawnAttacker(GameObject attacker, Vector3 position)
    {
        var attackerInstance = Instantiate(attacker, position, Quaternion.identity);
        var spawnCoolDown = attackerInstance.GetComponent<AttackerStats>().GetSpawnCoolDown();
        
        Delay(spawnCoolDown, () =>
        {
            SpawnAttacker(attacker, position);
        });
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

    public void AddCityHealth(int health)
    {
        playerCityHealth += health;
        OnPlayerCityHealthChanged?.Invoke(playerCityHealth);
    }

    public void RemoveCityHealth(int health)
    {
        playerCityHealth -= health;
        OnPlayerCityHealthChanged?.Invoke(playerCityHealth);
    }

    // Getters and Setters

    public bool CanAfford(int credits)
    {
        return playerCredits >= credits;
    }

    public bool IsWaveActive()
    {
        return isWaveActive;
    }

    public bool IsWaveComplete()
    {
        return isWaveComplete;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public void StartNextWave()
    {
        StartCoroutine(StartWave());
    }

    public void SetWaveCooldown(float cooldown)
    {
        waveCooldown = cooldown;
    }


    public void SetWaveNumber(int wave)
    {
        waveNumber = wave;
    }

    public void SetPlayerCredits(int credits)
    {
        playerCredits = credits;
    }
}
