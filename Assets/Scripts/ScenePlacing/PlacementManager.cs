using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Scripts.Utils.InstantiateUtils;

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    private Material previewMaterialValid;
    [SerializeField]
    private Material previewMaterialInvalid;
    [SerializeField]
    private GameObject objectToPlace;
    private GameObject objectInstance;
    private GameObject objectPreview;
    private IPlaceable placeable;
    [SerializeField]
    private LayerMask unplaceableLayer;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private LayerMask towerLayer;
    private bool isManaging = false;
    private bool isPreviewing = false;
    private bool canBePlaced = false;
    private bool isRemoving = false;

    public event Action OnPlaceObject;
    public event Action OnCancelManaging;
    public event Action OnStartManaging;

    // Singleton

    public static PlacementManager Instance { get; private set; }

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

    private void Update()
    {
        // If the player is not in plac
        if (!isManaging) return;

        if (isRemoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Removing();
            }
        }
        if (isPreviewing)
        {
            Preview();
        }

        if (Input.GetMouseButtonDown(0) && canBePlaced)
        {
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();

            if (isOverUI)
            {
                Debug.Log("Over UI");
                return;
            }

            OnPlace();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel();
        }
    }

    public void StartPlacing(GameObject objectToAssign)
    {
        AssignObject(objectToAssign);
        isManaging = true;
        isPreviewing = true;
        OnStartManaging?.Invoke();
    }
    public void AssignObject(GameObject objectToAssign)
    {
        objectToPlace = objectToAssign;
        placeable = objectToPlace.GetComponent<IPlaceable>();

        var preview = objectToPlace.GetComponent<IPlaceable>().ObjectPreview;
        objectPreview = InstantiatePrefab(preview, Vector3.zero, name: objectToPlace.name.Replace("(Clone)", ""));

        // Tower range line
        var towerRangeLine = objectPreview.GetComponentInChildren<TowerRangeLineHandler>();
        towerRangeLine.SetTowerStats(objectToPlace.GetComponent<TowerStats>());

        objectPreview.SetActive(true);
    }

    public void Preview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
        {
            var currentPosition = hit.point;
            var snapValue = 0.5f;
            // Calculate snapped position
            float snappedX = Snap(currentPosition.x, snapValue);
            float snappedY = Snap(currentPosition.y, snapValue);
            float snappedZ = Snap(currentPosition.z, snapValue);

            var snappedPosition = new Vector3(snappedX, snappedY, snappedZ);
            objectPreview.transform.position = snappedPosition;

            Debug.Log("Snapped position: " + snappedPosition);

            var renderer = objectPreview.GetComponentInChildren<Renderer>();
            while (renderer == null)
            {
                renderer = objectPreview.GetComponentInChildren<Renderer>();
            }
            if (Physics.CheckSphere(snappedPosition, placeable.ObjectRadius, unplaceableLayer))
            {
                Debug.Log("Invalid");
                canBePlaced = false;

                renderer.sharedMaterial = previewMaterialInvalid;
            }
            else
            {
                Debug.Log("Valid");
                canBePlaced = true;

                renderer.sharedMaterial = previewMaterialValid;
            }
        }
    }
    // Snapping function
    float Snap(float value, float snapValue)
    {
        return Mathf.Round(value / snapValue) * snapValue;
    }

    public void OnPlace()
    {
        ScenePlacingBehaviour.Instance.RemoveCredits(objectToPlace.GetComponent<TowerStats>().GetCost());

        objectInstance = InstantiatePrefab(objectToPlace, objectPreview.transform.position, objectPreview.transform.rotation);
        objectInstance.SetActive(true);
        Destroy(objectPreview);

        var costToPlaceAnother = objectInstance.GetComponent<TowerStats>().GetCost();
        if (!ScenePlacingBehaviour.Instance.CanAfford(costToPlaceAnother))
        {
            OnEnd();
            OnPlaceObject?.Invoke();
            Debug.Log("Not enough credits");
            return;
        }
        OnPlaceObject?.Invoke();
        StartPlacing(objectToPlace);
    }

    public void SwitchRemoving()
    {
        isRemoving = true;
        isManaging = true;
        OnStartManaging?.Invoke();
    }

    private void Removing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, towerLayer))
        {
            var hitObject = hit.collider.gameObject.transform.parent.gameObject;
            while (hitObject.transform.parent != null)
            {
                hitObject = hitObject.transform.parent.gameObject;
            }
            Debug.Log(hitObject.name);
            if (hitObject != null)
            {
                OnRemove(hitObject);
            }
        }
    }

    public void OnRemove(GameObject objectToRemove)
    {
        if (objectToRemove == null)
        {
            Debug.LogError("Object to remove is null");
            return;
        }
        var objectToRemoveStats = objectToRemove.GetComponent<TowerStats>();
        float amountToRefund = Mathf.Round(objectToRemoveStats.GetCost() * ((float)objectToRemoveStats.GetHealth() / (float)objectToRemoveStats.GetMaxHealth()));
        Debug.Log("Refund: " + amountToRefund);
        ScenePlacingBehaviour.Instance.AddCredits(amountToRefund);
        Destroy(objectToRemove);
        Debug.Log("OnRemove");
    }

    public void OnCancel()
    {
        if (isPreviewing)
        {
            Destroy(objectPreview);
        }
        OnEnd();
        Debug.Log("OnCancel");
        OnCancelManaging?.Invoke();
    }
    private void OnEnd()
    {
        isManaging = false;
        isPreviewing = false;
        canBePlaced = false;
        isRemoving = false;
        Debug.Log("OnEnd");
    }
}
