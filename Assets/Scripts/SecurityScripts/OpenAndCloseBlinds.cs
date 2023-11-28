using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//script for open and close blinds when player or thief enter and exit in trigger zone
public class OpenAndCloseBlinds : MonoBehaviour
{
    private bool isOpen, stopOpen;

    public float speedOpen = 5f;
    public Transform blinds;

    Vector3 targetScaleBlinds = new Vector3(1f, 0.15f, 1f);
    Vector3 startScale;

    private void Start()
    {
        startScale = blinds.localScale;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NavMeshAgent>() || other.GetComponent<CharacterController>())
        {
                isOpen = true;
                stopOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NavMeshAgent>() || other.GetComponent<CharacterController>())
        {
            isOpen = false;
            stopOpen = true;
        }
    }

    private void Update()
    {
        if (!isOpen && stopOpen)
        {
            blinds.localScale = Vector3.Lerp(blinds.localScale, startScale, speedOpen * Time.deltaTime);
            if (blinds.localScale == startScale)
            {
                isOpen = true;
                stopOpen = false;
            }
        }
        else if (isOpen && stopOpen)
        {

            blinds.localScale = Vector3.Lerp(blinds.localScale, targetScaleBlinds, speedOpen * Time.deltaTime);
            if (blinds.localScale == targetScaleBlinds)
            {
                isOpen = false;
                stopOpen = false;
            }
        }
    }
}
