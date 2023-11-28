using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarLookAtCamera : MonoBehaviour
{
    public GameObject playerCamera;
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        transform.LookAt(playerCamera.transform);
    }
}
