using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigHandResize : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            other.transform.GetChild(0).GetComponent<BuildManager>().distanceRay = 10f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            other.transform.GetChild(0).GetComponent<BuildManager>().distanceRay = 6f;
        }
    }
}
