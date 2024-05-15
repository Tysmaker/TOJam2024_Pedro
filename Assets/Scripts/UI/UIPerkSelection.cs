using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPerkSelection : MonoBehaviour
{

    private List<IPerkable> availableTowerPerks = new List<IPerkable>();
    private List<IPerkable> randomlyChosenPerks = new List<IPerkable>();
    [SerializeField]
    private Transform anchor;

    private int perkCount = 0;
    private GameObject currentTower;

    // Booleans
    private bool isSelectingPerks = false;
    public bool IsSelectionPhaseComplete { get; private set; } = false;


    // UI Elements
    [SerializeField] private GameObject perkButtonPrefab;
    [SerializeField] private Transform perkButtonContainer;

    // UI Tower info
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private GameObject towerInfoPrefab;
    private Dictionary<string, string> towerInfo = new Dictionary<string, string>();
    [SerializeField] private Transform towerInfoContainer;

    // Temporary Gameplay progression

    [SerializeField] private GameObject nextPhase;
    [SerializeField] private GameObject thisPhase;


    private void Awake()
    {
        GameObject perkResources = Resources.Load<GameObject>("TowerPerks/Tier01");
        if (perkResources == null)
        {
            Debug.LogError("Perk Resources not found");
            return;
        }
        var perks = perkResources.GetComponents<IPerkable>();
        foreach (var perk in perks)
        {
            if (perk is IPerkable) availableTowerPerks.Add(perk);
        }
        // randomlyChosenPerks = GetRandomPerks();

        if (!PlayerTowersManager.IsInitialized)
        {
            PlayerTowersManager.Init(anchor);
        }
    }

    private void Start()
    {
        perkCount = 0;
        foreach (var tower in PlayerTowersManager.playerTowers)
        {
            StartCoroutine(WaitForPerkSelection(tower.Value.gameObject, tower.Key));
        }
    }

    private IEnumerator WaitForPerkSelection(GameObject tower, string towerName)
    {
        while (isSelectingPerks)
        {
            yield return null;
        }
        isSelectingPerks = true;
        randomlyChosenPerks = GetRandomPerks();
        AssignCurrentTower(tower, towerName);
        DisableTowes();
        CreateUI();
    }
    private void AssignCurrentTower(GameObject tower, string towerName)
    {
        currentTower = tower;
        currentTower.name = towerName;
        Debug.Log("Perks Selected");
    }

    private void DisableTowes()
    {
        if (IsSelectionPhaseComplete)
        {
            foreach (var tower in PlayerTowersManager.playerTowers)
            {
                tower.Value.gameObject.SetActive(false);
            }
            return;
        }

        foreach (var t in PlayerTowersManager.playerTowers)
        {
            if (t.Key != currentTower.name)
            {
                t.Value.gameObject.SetActive(false);
            }
            else
            {
                t.Value.gameObject.SetActive(true);
            }
        }
    }


    private List<IPerkable> GetRandomPerks()
    {
        List<IPerkable> randomPerks = new List<IPerkable>();
        var towerPerks = new List<IPerkable>(availableTowerPerks);
        for (int i = 0; i < 3; i++)
        {
            var randomIndex = Random.Range(0, towerPerks.Count);
            randomPerks.Add(towerPerks[randomIndex]);
            towerPerks.RemoveAt(randomIndex);
        }
        return randomPerks;
    }

    private void CreateUI()
    {
        var towerPerksHandler = currentTower.GetComponent<TowerPerksHandler>();
        CreateTowerInfoUI();
        foreach (var perk in randomlyChosenPerks)
        {
            var perkButton = Instantiate(perkButtonPrefab, perkButtonContainer);
            var uiPerk = perkButton.GetComponent<UIPerk>();
            uiPerk.UpdateUI(perk.Name, perk.Description, perk.Cost);
            uiPerk.SetButtonAction(() => AddPerk(perk));
        }
    }
    private void CreateTowerInfoUI()
    {
        // Set Dictionary
        var towerStats = currentTower.GetComponent<TowerStats>();
        towerStats.GetTowerInfo(towerInfo);

        towerName.text = towerStats.GetTowerName();
        foreach (var info in towerInfo)
        {
            var towerInfoUI = Instantiate(towerInfoPrefab, towerInfoContainer);
            var uiTowerInfo = towerInfoUI.GetComponent<UITowerStatsInfo>();
            uiTowerInfo.SetInfo(info.Key, info.Value);
        }
    }

    private void ClearUI()
    {
        foreach (Transform child in perkButtonContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in towerInfoContainer)
        {
            Destroy(child.gameObject);
        }
        towerInfo.Clear();
    }

    private void AddPerk(IPerkable perk)
    {
        perkCount++;
        var towerStats = currentTower.GetComponent<TowerStats>();
        var towerPerksHandler = currentTower.GetComponent<TowerPerksHandler>();
        towerPerksHandler.AddPerk(perk);
        PlayerTowersManager.ApplyPerk(perk, towerStats);
        PerkSelected();
    }

    private void PerkSelected()
    {
        isSelectingPerks = false;
        ClearUI();
        if (perkCount == PlayerTowersManager.playerTowers.Count)
        {
            PerkSelectionComplete();
        }
    }

    private void PerkSelectionComplete()
    {
        IsSelectionPhaseComplete = true;
        foreach (var tower in PlayerTowersManager.playerTowers)
        {
            tower.Value.gameObject.SetActive(false);
        }
        nextPhase.SetActive(true);
        thisPhase.SetActive(false);
        Debug.Log("Perk Selection Complete");
    }


}
