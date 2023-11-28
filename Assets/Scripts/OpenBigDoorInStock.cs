using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenBigDoorInStock : MonoBehaviour
{
    private bool isOpen, stopOpen;

    public float speedOpen = 5f;

    public Transform door;
    [SerializeField] bool isWindowDoor;
    Vector3 targetPositioneDoor;


    Vector3 startPosition;

    private void Start()
    {
        startPosition = door.position;
        targetPositioneDoor = new Vector3 (startPosition.x, 3.5f, startPosition.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() || other.GetComponent<NavMeshAgent>())
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


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() || other.GetComponent<NavMeshAgent>())
        {
            isOpen = false;
            stopOpen = true;
        }
    }

    private void Update()
    {
        if (!isOpen && stopOpen)
        {
            door.position = Vector3.Lerp(door.position, startPosition, speedOpen * Time.deltaTime);

            if (door.position == startPosition)
            {
                isOpen = false;
                stopOpen = false;
            }
        }
        else if (isOpen && stopOpen)
        {
            door.position = Vector3.Lerp(door.position, targetPositioneDoor, speedOpen * Time.deltaTime);
            if (door.position == targetPositioneDoor)
            {
                isOpen = true;
                stopOpen = false;
            }
            
        }
    }
}
