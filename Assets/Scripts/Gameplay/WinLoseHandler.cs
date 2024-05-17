
using UnityEngine;


public class WinLoseHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject victoryScreen;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);

        GameplayManager.Instance.OnGameOver += SetGameOverScreen;
        GameplayManager.Instance.OnVictory += SetVictoryScreen;
    }

    private void SetVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void SetGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    private void OnDestroy()
    {
         GameplayManager.Instance.OnGameOver -= SetGameOverScreen;
        GameplayManager.Instance.OnVictory -= SetVictoryScreen;
    }


}
