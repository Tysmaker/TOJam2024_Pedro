using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private int gameTimeInSeconds = 300;
    private Coroutine gameTimer;
    private bool isPlayerDefending = false;

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

    public void IsPlayerDefending(bool isDefending)
    {
        isPlayerDefending = isDefending;
    }

    public void StartGame()
    {
        Debug.Log("Game Started");
        if (isPlayerDefending)
        {
            BaseBehaviour.OnDeath += GameOver;
        }
        else
        {
            BaseBehaviour.OnDeath += Victory;
        }
        gameTimer = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameTimeInSeconds);
        Victory();
    }

    private void Victory()
    {
        OnVictory?.Invoke();
        Debug.Log("Victory");
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        StopCoroutine(gameTimer);  
        Destroy(_instance);

        Debug.Log("Game Over");
    }

    public int GetInitialGameTime()
    {
        return gameTimeInSeconds;
    }

    private void OnDestroy()
    {
        BaseBehaviour.OnDeath -= GameOver;
        BaseBehaviour.OnDeath -= Victory;
    }
}
