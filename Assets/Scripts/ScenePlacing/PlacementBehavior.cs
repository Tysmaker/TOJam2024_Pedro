using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementBehavior : MonoBehaviour
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
    private bool isPlaced = false;
    private bool canBePlaced = false;
    private bool isRemoving = false;

    public void Start()
    {
        objectInstance = Instantiate(objectToPlace);
        placeable = objectInstance.GetComponent<IPlaceable>();
    }

    private void Update()
    {
        if (!isPlaced)
        {
            OnPreview();
        }

        if (Input.GetMouseButtonDown(0) && canBePlaced)
        {
            OnPlace();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRemove();
        }
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
        isPlaced = true;
        placeable.ObjectCollider.gameObject.layer = LayerMask.NameToLayer("Default");
        placeable.ObjectRenderer.sharedMaterial = placeable.DefaultMaterial;
        Debug.Log("OnPlace");
    }

    public void OnRemove()
    {
        Debug.Log("OnRemove");
    }
    public void OnCancel()
    {
        Debug.Log("OnCancel");
    }
}
