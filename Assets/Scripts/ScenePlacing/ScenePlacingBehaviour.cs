using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenePlacingBehaviour : MonoBehaviour
{
    public float PlayerCredits { get; private set; } = 100;
    [SerializeField] public TextMeshProUGUI creditsText;

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

        ///////////////////////////////////////////// TESTING  /////////////////////////////////////////////
        InitPrefabs();
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


    ///////////////////////////////////////////// TESTING  /////////////////////////////////////////////
    ///

    private void InitPrefabs()
    {
        PlayerTowersManager.Init(transform);
    }
}
