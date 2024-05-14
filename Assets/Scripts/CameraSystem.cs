using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemmachineVirtualCamera;

    [SerializeField] private float cameraMoveSpeed = 50f;

    [SerializeField] private bool useEdgeScrolling = false;

    [SerializeField] private bool useDragPan = false;

    [SerializeField] private float fieldOfViewMax = 50;

    [SerializeField] private float fieldOfViewMin = 10;


    //[SerializeField] private bool dragPanMoveActive;
    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView = 50;

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();

        if(useEdgeScrolling)
        {
            HandleCameraMovementEdgeScrolling();
        }
        

        if(useDragPan) 
        {
            HandleCameraMovementDragPan(); 
        }

        HandleCameraZoom();
    }


    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        //Keyboard Camera Movement
        if (Input.GetKey(KeyCode.W)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = +1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = -1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += inputDir * cameraMoveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        //Edge Scrolling
       
            int edgeScrollSize = 20;
            if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
            if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f;
        

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += inputDir * cameraMoveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementDragPan()
    {    
        Vector3 inputDir = new Vector3(0, 0, 0);

        //MouseDrag
        if (Input.GetMouseButtonDown(1))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        { 
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 2f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += inputDir * cameraMoveSpeed * Time.deltaTime;
    }

    private void HandleCameraZoom() 
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5;
            cameraMoveSpeed = 1;
        }

        if(Input.mouseScrollDelta.y < 0 )
        {
            targetFieldOfView += 5;
            cameraMoveSpeed = 5;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);


        float zoomSpeed = 5f;
        cinemmachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemmachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
    }
}
