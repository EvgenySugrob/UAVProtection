using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Camera.main.GetComponent<CheckYourWork>().TimerOff();
    }
}
