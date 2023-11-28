using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMainDoor : MonoBehaviour
{
    private bool isOpen, stopOpen;

    public float speedOpen = 2f;

    public Transform door,door1;
    Quaternion targetAngle = Quaternion.Euler(0.0f, -90f, 0.0f);
    Quaternion targetAngle1 = Quaternion.Euler(0.0f, 90f, 0.0f);

    Quaternion startRotation;
    Quaternion startRotation1;

    private void Start()
    {
        startRotation = door.rotation;
        startRotation1 = door1.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<SensorDetect>() && !other.GetComponent<SmokeSensorDetect>() && !other.GetComponent<SecureDetect>() && !other.GetComponent<FireActivationDiactivation>()
            && !other.GetComponent<SmokeDioxidActivationDiactivation>())
        {
            isOpen = true;
            stopOpen = true;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<SensorDetect>() && !other.GetComponent<SmokeSensorDetect>() && !other.GetComponent<SecureDetect>() && !other.GetComponent<FireActivationDiactivation>()
            && !other.GetComponent<SmokeDioxidActivationDiactivation>())
        {
            isOpen = false;
            stopOpen = true;
        }
    }

    private void Update()
    {
        if (!isOpen && stopOpen)
        {
            door.rotation = Quaternion.Lerp(door.rotation, startRotation, speedOpen * Time.deltaTime);
            door1.rotation = Quaternion.Lerp(door1.rotation, startRotation, speedOpen * Time.deltaTime);
            if (door.rotation == startRotation)
            {
                isOpen = true;
                stopOpen = false;
            }
        }
        else if (isOpen && stopOpen)
        {
            door.rotation = Quaternion.Lerp(door.rotation, targetAngle, speedOpen * Time.deltaTime);
            door1.rotation = Quaternion.Lerp(door1.rotation, targetAngle1, speedOpen * Time.deltaTime);
            if (door.rotation == targetAngle)
            {
                isOpen = false;
                stopOpen = false;
            }
        }
    }
}
