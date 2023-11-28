using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonControler : MonoBehaviour
{
    public float sensX, sensY;

    public Transform orentation;

    float xRotation, yRotation;

    public bool isRotationOn;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        orentation.localRotation = Quaternion.Euler(0, 90f, 0);
    }

    private void Update()
    {
        if (!isRotationOn)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orentation.localRotation = Quaternion.Euler(0, yRotation, 0);
        }   
    }

    public void RotationOnOff()
    {
        isRotationOn = !isRotationOn;
    }
}
