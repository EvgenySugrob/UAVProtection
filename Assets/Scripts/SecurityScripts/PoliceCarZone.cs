using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script foe stop moving police car
//Should be attached on PoliceZone object on scene
public class PoliceCarZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Animator>())
        {
            other.gameObject.GetComponent<Animator>().SetBool("IsStop", true);
            foreach (Transform child in other.transform)
                if (child.GetComponent<Animator>()) 
                    child.GetComponent<Animator>().SetBool("IsStop", true);
        }

    }
}
