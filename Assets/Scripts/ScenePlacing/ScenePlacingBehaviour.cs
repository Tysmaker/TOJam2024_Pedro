using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenePlacingBehaviour : MonoBehaviour
{
    [SerializeField]
    private float playerCredits = 500;
    [SerializeField] public TextMeshProUGUI creditsText;

    // Temporary Gameplay progression

    [SerializeField] private GameObject defenderGameplay;	
    [SerializeField] private GameObject attackerGameplay;
    [SerializeField] private GameObject thisPhase;
    [SerializeField] private GameObject placingTowersPhase;

    // Singleton
    public static ScenePlacingBehaviour Instance { get; private set; }

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
        UpdateUI();
    }

    public void StartGame()
    {
        // Start the game through the Defender Gameplay Manager
        thisPhase.SetActive(false);
        var chance = Random.Range(0, 2);
        if (chance == 0)
        {
            attackerGameplay.SetActive(true);
            placingTowersPhase.SetActive(false);
            GameplayManager.Instance.IsPlayerDefending(false);
            GameplayManager.Instance.StartGame();
        }
        else
        {
            defenderGameplay.SetActive(true);
            GameplayManager.Instance.IsPlayerDefending(true);
            GameplayManager.Instance.StartGame();
        }
    }

    public void AddCredits(float credits)
    {
        playerCredits += credits;
        UpdateUI();
    }

    public void RemoveCredits(float credits)
    {
        playerCredits -= credits;
        UpdateUI();
    }
    public void SetCredits(float credits)
    {
        playerCredits = credits;
        UpdateUI();
    }

    public bool CanAfford(float credits)
    {
        return playerCredits >= credits;
    }

    private void UpdateUI()
    {
        creditsText.text = playerCredits.ToString();
    }
}
