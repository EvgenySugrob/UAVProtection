using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script for adding glass ident(zoneName)
public class GlassIdent : MonoBehaviour
{
    public string zoneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SetInZones>())
        {
            zoneName = other.gameObject.name;
            if (gameObject.GetComponent<Rigidbody>())
                Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }
}
