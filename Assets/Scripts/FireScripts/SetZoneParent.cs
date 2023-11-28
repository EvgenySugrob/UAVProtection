using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetZoneParent : MonoBehaviour
{
    public List<GameObject> zoneObject;

    //Assigning a location zone to fire detectors
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<SphereCollider>() && !other.gameObject.GetComponent<PrefabBuild>()
            && !zoneObject.Contains(other.gameObject) && other.transform.GetChild(2).gameObject.GetComponent<SensorDetect>()
            && other.transform.GetChild(2).gameObject.GetComponent<SensorDetect>().zoneName == "")
        {
            other.gameObject.transform.parent = transform;
            zoneObject.Add(other.gameObject);
            other.transform.GetChild(2).gameObject.GetComponent<SensorDetect>().zoneName = gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SphereCollider>() && !other.gameObject.GetComponent<PrefabBuild>()
            && zoneObject.Contains(other.gameObject) && other.transform.GetChild(2).gameObject.GetComponent <SensorDetect>()
            && other.transform.GetChild(2).gameObject.GetComponent<SensorDetect>().zoneName != "")
        {
            other.gameObject.transform.parent = null;
            zoneObject.Remove(other.gameObject);
            other.transform.GetChild(2).gameObject.GetComponent<SensorDetect>().zoneName = "";
        }
    }

}
