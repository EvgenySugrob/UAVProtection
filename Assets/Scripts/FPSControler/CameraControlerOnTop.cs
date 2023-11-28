using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlerOnTop : MonoBehaviour
{
    public Transform cameraTransform;

    public float movementSpeed, movementTime, rotationAmount;

    public Vector3 newPosition;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public Quaternion newRotation;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    private Vector3 startAllPosition = new Vector3(0f,0f,0f);
    private Quaternion startAllRotation = Quaternion.Euler(0f, 0f, 0f);

    private Vector3 cameraPositionVector;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        cameraPositionVector = newZoom = cameraTransform.localPosition;
    }
    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            newZoom.y = Mathf.Clamp(newZoom.y, 5f, 15f);
            newZoom.z = Mathf.Clamp(newZoom.z, -15f, -5f);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up,Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray,out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                newPosition.x = Mathf.Clamp(newPosition.x,-25f,25f);
                newPosition.z = Mathf.Clamp(newPosition.z, -25f, 45f);
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f)); 
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
        cameraTransform.transform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
    }

    public void SetPosition()
    {
        newPosition = dragStartPosition = dragCurrentPosition = rotateStartPosition = rotateCurrentPosition = startAllPosition;
        newZoom = cameraPositionVector;
        newRotation = startAllRotation;
    }
}
