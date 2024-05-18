using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackerManager : MonoBehaviour
{
    private List<AttackerBehaviour> activeEnemies = new List<AttackerBehaviour>();
    private List<GameObject> spawnQueue = new List<GameObject>();
    [SerializeField]
    private Transform targetArea;
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();

    private bool isGameOver = false;

    // Placement
    [SerializeField]
    private GameObject objectPreview;
    private GameObject currentPreview;
    private GameObject objectToPlace;
    [SerializeField]
    private LayerMask placeableLayer;
    [SerializeField]
    private Material previewMaterialValid;
    [SerializeField]
    private Material previewMaterialInvalid;
    private bool isPreviewing = false;
    private bool isValid = false;

    // UI Elements
    [SerializeField]
    private List<Button> attackerButtons = new List<Button>();
    private Button activeButton;


    // Events

    // Singleton
    public static AttackerManager Instance { get; private set; }

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
        SetButtons();
    }

    private void Update()
    {
        if (!isPreviewing) return;

        Preview();

        // Check if not over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && isValid)
        {
            PlaceAttacker();
        }
    }

    private void StartPlacingEnemy(int index)
    {
        if (isPreviewing) 
        {
            Destroy(currentPreview);
        }
        isPreviewing = true;
        objectToPlace = enemyPrefabs[index];
        //SetButtonInteractable(false);
        currentPreview = Instantiate(objectPreview);
    }

    private void SetActiveButton(Button button)
    {
        activeButton = button;
    }

    private void Preview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableLayer))
        {
            currentPreview.transform.position = hit.point;
            isValid = true;
            currentPreview.GetComponent<MeshRenderer>().material = previewMaterialValid;
        }
        else
        {
            isValid = false;
            currentPreview.GetComponent<MeshRenderer>().material = previewMaterialInvalid;
        }
    }

    private void SetButtons()
    {
        for (int i = 0; i < attackerButtons.Count; i++)
        {
            int index = i;
            var uiPlaceButtons = attackerButtons[i].GetComponent<UIAttackerPlaceButton>();
            var buttonTMP = attackerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            var attackerStats = enemyPrefabs[i].GetComponent<AttackerStats>();

            uiPlaceButtons.SetAttackerStats(attackerStats);
            buttonTMP.text = attackerStats.GetName();

            attackerButtons[i].onClick.AddListener(() => StartPlacingEnemy(index));
            attackerButtons[i].onClick.AddListener(() => SetActiveButton(attackerButtons[index]));
        }
    }

    private void SetButtonInteractable(bool value)
    {
        foreach (var button in attackerButtons)
        {
            button.interactable = value;
        }
    }

    private void PlaceAttacker()
    {
        var attackerStats = objectToPlace.GetComponent<AttackerStats>();

        // Instantiate the attacker based on the amount spawn at once
        for (int i = 0; i < attackerStats.GetAmountSpawnAtOnce(); i++)
        {
            var instance = Instantiate(objectToPlace, currentPreview.transform.position, Quaternion.AngleAxis(180, Vector3.up));
            var attacker = instance.GetComponent<AttackerBehaviour>();

            instance.SetActive(true);
            attacker.SetEndZone(targetArea.gameObject);
            AddEnemyToQueue(attacker.gameObject);
        }

        Destroy(currentPreview);
        PlacementEnded();
    }

    private void AddEnemyToQueue(GameObject enemyPrefab)
    {
        spawnQueue.Add(enemyPrefab);
    }

    private void PlacementEnded()
    {
        isPreviewing = false;
        //SetButtonInteractable(true);
        var uiPlaceButton = activeButton.GetComponent<UIAttackerPlaceButton>();
        uiPlaceButton.StartCooldown();
    }
}
