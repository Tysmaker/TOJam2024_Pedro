using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPerkSelection : MonoBehaviour
{

    private List<IPerkable> availableTowerPerks = new List<IPerkable>();
    private IPerkable[] randomlyChosenPerks = new IPerkable[3];
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private int perkCount = 0;

    private GameObject currentTower;

    private bool isSelectiongPerks = false;

    // UI Elements
    [SerializeField] private GameObject perkButtonPrefab;
    [SerializeField] private Transform perkButtonContainer;


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
        randomlyChosenPerks = GetRandomPerks();
        perkCount = randomlyChosenPerks.Length;

        if (!PlayerTowersManager.IsInitialized)
        {
            PlayerTowersManager.Init(anchor);
        }
    }

    private void Start()
    {
        foreach (var tower in PlayerTowersManager.playerTowers)
        {
            StartCoroutine(WaitForPerkSelection(tower.Value.gameObject, tower.Key));
        }
    }

    private IEnumerator WaitForPerkSelection(GameObject tower, string towerName)
    {

        while (isSelectiongPerks)
        {
            yield return null;
        }
        UpdateUI();
        currentTower = tower;
        currentTower.name = towerName;
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
        CreateUI();
        isSelectiongPerks = true;
        Debug.Log("Perks Selected");
    }

    private IPerkable[] GetRandomPerks()
    {
        IPerkable[] randomPerks = new IPerkable[3];
        for (int i = 0; i < 3; i++)
        {
            var randomIndex = Random.Range(0, availableTowerPerks.Count);
            randomPerks[i] = availableTowerPerks[randomIndex];
            availableTowerPerks.RemoveAt(randomIndex);
        }
        return randomPerks;
    }

    private void CreateUI()
    {
        var towerPerksHandler = currentTower.GetComponent<TowerPerksHandler>();
        foreach (var perk in randomlyChosenPerks)
        {
            var perkButton = Instantiate(perkButtonPrefab, perkButtonContainer);
            var uiPerk = perkButton.GetComponent<UIPerk>();
            uiPerk.UpdateUI(perk.Name, perk.Description, perk.Cost);
            uiPerk.SetButtonAction(() => AddPerk(perk));
        }
    }

    private void AddPerk(IPerkable perk)
    {
        var towerStats = currentTower.GetComponent<TowerStats>();
        var towerPerksHandler = currentTower.GetComponent<TowerPerksHandler>();
        towerPerksHandler.AddPerk(perk);
        PlayerTowersManager.ApplyPerk(perk, towerStats);
        PerkSelected();
    }

    private void PerkSelected()
    {
        isSelectiongPerks = false;
        foreach (Transform child in perkButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateUI()
    {
    }


}
