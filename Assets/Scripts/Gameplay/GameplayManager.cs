using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private int gameTimeInSeconds = 300;
    private Coroutine gameTimer;


    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject victoryScreen;

    // Events

    public event System.Action OnGameOver;
    public event System.Action OnVictory;


    // Singleton
    private static GameplayManager _instance;
    public static GameplayManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Subscribe to events
        BaseBehaviour.OnDeath += GameOver;
    }

    public void StartGame()
    {
        Debug.Log("Game Started");
        gameTimer = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameTimeInSeconds);
        Victory();
    }

    private void Victory()
    {
        victoryScreen.SetActive(true);

        OnVictory?.Invoke();
        Debug.Log("Victory");
    }

    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        OnGameOver?.Invoke();
        StopCoroutine(gameTimer);  

        Debug.Log("Game Over");
    }

    public int GetInitialGameTime()
    {
        return gameTimeInSeconds;
    }
}
