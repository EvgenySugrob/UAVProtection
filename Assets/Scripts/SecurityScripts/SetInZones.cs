using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class SetInZones : MonoBehaviour
{
    /*Adding sensors to the list of the placed zone, attaching to the zones where the sensor is supposed to be placed*/
    public List<GameObject> zoneObjects;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NavMeshModifierVolume>() && other.GetComponent<SphereCollider>()
            && !other.gameObject.GetComponent<PrefabBuild>() && !zoneObjects.Contains(other.gameObject))
        {
            other.gameObject.transform.parent = transform;
            zoneObjects.Add(other.gameObject);
            if (other.gameObject.transform.GetChild(2).gameObject.GetComponent<SecureDetect>())
                other.gameObject.transform.GetChild(2).gameObject.GetComponent<SecureDetect>().zoneName = gameObject.name;
            if (other.gameObject.transform.GetChild(2).gameObject.GetComponent<SecureGlass>())
                other.gameObject.transform.GetChild(2).gameObject.GetComponent<SecureGlass>().zoneName = gameObject.name;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<NavMeshAgent>() && other.gameObject.GetComponent<Walk>())
        {
            other.gameObject.GetComponent<Walk>().zoneName = gameObject.name;
        }
    }
}
