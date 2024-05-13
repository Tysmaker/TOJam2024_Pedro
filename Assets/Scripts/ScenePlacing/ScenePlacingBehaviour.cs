using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenePlacingBehaviour : MonoBehaviour
{
    public float PlayerCredits { get; private set; } = 100;
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
        PlayerCredits += credits;
        UpdateUI();
    }

    public void RemoveCredits(float credits)
    {
        PlayerCredits -= credits;
        UpdateUI();
    }
    public void SetCredits(float credits)
    {
        PlayerCredits = credits;
        UpdateUI();
    }

    public bool CanAfford(float credits)
    {
        return PlayerCredits >= credits;
    }

    private void UpdateUI()
    {
        creditsText.text = PlayerCredits.ToString();
    }
}
