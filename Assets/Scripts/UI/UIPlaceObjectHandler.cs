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
        PlacementManager.Instance.OnPlaceObject += OnFinish;
        PlacementManager.Instance.OnCancelManaging += OnFinish;
        PlacementManager.Instance.OnStartManaging += OnStart;
    }

    public void StartPlacingObject(GameObject prefab)
    {
        prefabToPlace = prefab;
        PlacementManager.Instance.StartPlacing(prefabToPlace);
    } 

    private void OnStart()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    private void OnFinish()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
