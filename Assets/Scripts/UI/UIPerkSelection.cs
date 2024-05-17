using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPerkSelection : MonoBehaviour
{

    private List<Perk> availableTowerPerks = new List<Perk>();
    private List<Perk> randomlyChosenPerks = new List<Perk>();
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
    private Dictionary<string, UITowerStatsInfo> towerInfoInstances = new Dictionary<string, UITowerStatsInfo>();
    [SerializeField] private Transform towerInfoContainer;

    // Temporary Gameplay progression

    [SerializeField] private GameObject nextPhase;
    [SerializeField] private GameObject thisPhase;


    private void Awake()
    {
        var perks = Resources.LoadAll<Perk>("TowerPerks/Tier01");

        if (perks == null || perks.Length == 0)
        {
            Debug.LogError("Perk Resources not found");
            return;
        }

        foreach (var perk in perks)
        {
            if (perk is Perk) availableTowerPerks.Add(perk);
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


    private List<Perk> GetRandomPerks()
    {
        List<Perk> randomPerks = new List<Perk>();
        var towerPerks = new List<Perk>(availableTowerPerks);
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
        CreateTowerInfoUI();
        foreach (var perk in randomlyChosenPerks)
        {
            var perkButton = Instantiate(perkButtonPrefab, perkButtonContainer);
            var uiPerk = perkButton.GetComponent<UIPerk>();

            var eventTrigger = perkButton.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = perkButton.AddComponent<EventTrigger>();
            }
            AddEventTrigger(perkButton.GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, (data) => PreviewPerkChanges(data, perk));
            AddEventTrigger(perkButton.GetComponent<EventTrigger>(), EventTriggerType.PointerExit, (data) => SetInitialTowerInfo());

            uiPerk.UpdateUI(perk.GetName(), perk.GetDescription(), perk.GetCost());
            uiPerk.SetButtonAction(() => AddPerk(perk));
        }
    }

    private void AddEventTrigger(EventTrigger eventTrigger, EventTriggerType triggerType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => action(data));
        eventTrigger.triggers.Add(entry);
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
            towerInfoInstances.Add(info.Key, uiTowerInfo);
        }
    }

    private void PreviewPerkChanges(BaseEventData data, Perk perk)
    {

        // Get the perk type and tower stats
        var perkType = perk.GetPerkType();
        var towerStats = currentTower.GetComponent<TowerStats>();
        float valueWithPerk;
        string valueString;

        if (perkType == TowerStats.PerkTypes.AttackSpeed)
        {
            valueWithPerk = towerStats.GetTowerInfo(perkType) / perk.GetValueToIncrease();
            valueString = valueWithPerk.ToString("N2") + " s/atk";
        }
        else
        {
            valueWithPerk = perk.GetValueToIncrease() + towerStats.GetTowerInfo(perkType);
            valueString = valueWithPerk.ToString();
        }

        // Calculate the cost with the perk and retrieve the UI elements
        var costWithPerk = perk.GetCost() + towerStats.GetCost();
        var uiTowerInfo = towerInfoInstances[perkType.ToString()];
        var uiTowerCost = towerInfoInstances["Cost"];

        // Set the UI elements
        uiTowerInfo.SetInfo(perkType.ToString(), valueString);
        uiTowerInfo.SetTextColour(Color.green);

        uiTowerCost.SetInfo("Cost", costWithPerk.ToString());
        uiTowerCost.SetTextColour(Color.green);
    }

    private void SetInitialTowerInfo()
    {
        foreach (var info in towerInfoInstances)
        {
            info.Value.SetInfo(info.Key, towerInfo[info.Key]);
            info.Value.SetTextColour(Color.white);
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
        towerInfoInstances.Clear();
    }

    private void AddPerk(Perk perk)
    {
        perkCount++;
        var towerStats = currentTower.GetComponent<TowerStats>();
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
