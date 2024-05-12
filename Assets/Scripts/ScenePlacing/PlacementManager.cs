using System;
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
            OnPreview();
        }

        if (Input.GetMouseButtonDown(0) && canBePlaced)
        {
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();

            if (isOverUI)
            {
                return;
            }
            
            OnPlace();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel();
        }
    }

    public void StartPlacing(GameObject objectToPlace)
    {
        AssignObject(objectToPlace);
        isManaging = true;
        isPreviewing = true;
        OnStartManaging?.Invoke();
    }
    public void AssignObject(GameObject objectToAssign)
    {
        objectToPlace = objectToAssign;
        objectInstance = InstantiatePrefab(objectToPlace, Vector3.zero, name: objectToPlace.name.Replace("(Clone)", ""));
        placeable = objectInstance.GetComponent<IPlaceable>();
        objectInstance.SetActive(true);
    }

    public void OnPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
        {

            objectInstance.transform.position = hit.point;
            placeable.ObjectCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            if (Physics.CheckSphere(hit.point, placeable.ObjectRadius, unplaceableLayer))
            {
                Debug.Log("Invalid");
                canBePlaced = false;
                objectInstance.transform.position = hit.point;
                placeable.ObjectRenderer.sharedMaterial = previewMaterialInvalid;
            }
            else
            {
                Debug.Log("Valid");
                canBePlaced = true;
                objectInstance.transform.position = hit.point;
                placeable.ObjectRenderer.sharedMaterial = previewMaterialValid;
            }
        }
    }

    public void OnPlace()
    {
        placeable.ObjectCollider.gameObject.layer = LayerMask.NameToLayer("Tower");
        placeable.ObjectRenderer.sharedMaterial = placeable.DefaultMaterial;
        OnEnd();
        OnPlaceObject?.Invoke();
        Debug.Log("OnPlace");
    }

    public void SwitchRemoving()
    {
        isRemoving = true;
        isManaging = true;
        OnStartManaging?.Invoke();
    }


    public void OnRemove(GameObject objectToRemove)
    {
        Destroy(objectToRemove);
        Debug.Log("OnRemove");
    }

    private void Removing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, towerLayer))
        {
            var hitObject = hit.collider.gameObject.transform.parent.gameObject;
            if (hitObject != null)
            {
                OnRemove(hitObject);
            }
        }
    }

    public void OnCancel()
    {
        if (isPreviewing)
        {
            Destroy(objectInstance);
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
