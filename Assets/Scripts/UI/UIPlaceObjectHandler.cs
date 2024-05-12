using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceObjectHandler : MonoBehaviour
{
    private GameObject prefabToPlace;
    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        PlacementManager.Instance.OnPlaceObject += FinishManaging;
        PlacementManager.Instance.OnCancelManaging += FinishManaging;
        PlacementManager.Instance.OnStartManaging += StartManaging;
    }

    public void StartPlacingObject(int buttonTowerIndex)
    {
        prefabToPlace = PlayerTowersManager.GetTowerPrefab(buttonTowerIndex);
        if (prefabToPlace == null)
        {
            Debug.LogError("UIPlaceObjectHandler: prefabToPlace is null");
            return;
        }
        PlacementManager.Instance.StartPlacing(prefabToPlace);
    }

    private void StartManaging()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    private void FinishManaging()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
