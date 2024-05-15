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

    [SerializeField] private GameObject nextPhase;	
    [SerializeField] private GameObject thisPhase;

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
        nextPhase.SetActive(true);
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
