using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenDoor : MonoBehaviour
{
    private bool isOpen, stopOpen;
    public bool needBrokeGlass = false; //переменная для отслеживания, что нужно сделать вору с окном, где 0 - открыть, 1 - разбить
    public float speedOpen = 5f;
    public GameObject glassWindow;
    public Transform door;
    public bool isWindowDoor;
    Quaternion targetAngleDoor = Quaternion.Euler(0.0f, 120f, 0.0f);
    Quaternion targetAngleWindow = Quaternion.Euler(0.0f, -120f, 0.0f);

    Quaternion startRotation;

    private void Start()
    {
        startRotation = door.localRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<NavMeshAgent>() || other.GetComponent<CharacterController>()) && !needBrokeGlass)
        {
            if (!isWindowDoor)
            {
                isOpen = true;
                stopOpen = true;
            }
            else
            {
                if (other.GetComponent<NavMeshAgent>())
                {
                    isOpen = true;
                    stopOpen = true;
                }
            }
        }
        if (other.GetComponent<NavMeshAgent>() && needBrokeGlass)
        {
            glassWindow.SetActive(false);
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.GetComponent<NavMeshAgent>() || other.GetComponent<CharacterController>()) && !needBrokeGlass)
        {
            isOpen = false;
            stopOpen = true;
        }
    }

    private void Update()
    {
        if (!isOpen && stopOpen)
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, startRotation, speedOpen * Time.deltaTime);
            if (door.localRotation == startRotation)
            {
                isOpen = true;
                stopOpen = false;
            }
        }
        else if (isOpen && stopOpen)
        {
            if (!isWindowDoor)
                door.localRotation = Quaternion.Lerp(door.localRotation, targetAngleDoor, speedOpen * Time.deltaTime);
            else
                door.localRotation = Quaternion.Lerp(door.localRotation, targetAngleWindow, speedOpen * Time.deltaTime);
            if (door.localRotation == targetAngleDoor || door.localRotation == targetAngleWindow)
            {
                isOpen = false;
                stopOpen = false;
            }
        }
    }
    public void ReturnGlass()
    {
        if (glassWindow != null)
            glassWindow.SetActive(true);
    }
}
