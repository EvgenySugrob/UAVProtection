using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RotationLightCarP : MonoBehaviour
{
    public GameObject blueLight, redLight;
    private void FixedUpdate()
    {
        blueLight.transform.Rotate(new Vector3(0, 45, 0) * Time.fixedDeltaTime*10); 
        redLight.transform.Rotate(new Vector3(0, -45, 0) * Time.fixedDeltaTime*10);
    }
}
