using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZoneSmokeParent : MonoBehaviour
{
    public List<GameObject> smokeZoneObject;

    //Assigning a Location Zone to Smoke Detectors
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<SphereCollider>() && !other.gameObject.GetComponent<PrefabBuild>()
            && !smokeZoneObject.Contains(other.gameObject) && other.transform.GetChild(2).gameObject.GetComponent<SmokeSensorDetect>()
            && other.transform.GetChild(2).gameObject.GetComponent<SmokeSensorDetect>().zoneName == "")
        {
            other.gameObject.transform.parent = transform;
            smokeZoneObject.Add(other.gameObject);
            other.transform.GetChild(2).gameObject.GetComponent<SmokeSensorDetect>().zoneName = gameObject.name;
        }
    }
     private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SphereCollider>() && !other.gameObject.GetComponent<PrefabBuild>()
            && smokeZoneObject.Contains(other.gameObject) && other.transform.GetChild(2).gameObject.GetComponent <SmokeSensorDetect>()
            && other.transform.GetChild(2).gameObject.GetComponent<SmokeSensorDetect>().zoneName != "")
        {
            other.gameObject.transform.parent = null;
            smokeZoneObject.Remove(other.gameObject);
            other.transform.GetChild(2).gameObject.GetComponent<SmokeSensorDetect>().zoneName = "";
        }
    }
}
