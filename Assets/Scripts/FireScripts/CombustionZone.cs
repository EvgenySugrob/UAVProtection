using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustionZone : MonoBehaviour
{
    public BoxCollider flamezone;
    public List<ParticleSystem> particleSystemsList;
    public List<ParticleSystem> particleSystemsSmokeList;

    public List<GameObject> dragObject;

    //public List<GameObject> fireObject;

    private void Start()
    {
        flamezone = GetComponent<BoxCollider>();
        
    }

    //The script weighs on the zones in which sensors and objects for interaction will be installed. Objects are assigned the name of the zone in which they are located.
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<FireActivationDiactivation>() && other.GetComponent<FireActivationDiactivation>().zoneName == "")
        {
            //fireObject.Add(other.transform.GetComponent<FireActivationDiactivation>().gameObject);

            particleSystemsList.Add(other.transform.GetChild(0).GetComponent<ParticleSystem>());
            other.transform.GetChild(0).GetComponent<ParticleSystem>().trigger.SetCollider(0, flamezone);
            other.GetComponent<FireActivationDiactivation>().SetZoneName(this.name);
            
        }
        if (other.GetComponent<BoxCollider>() && other.GetComponent<SmokeDioxidActivationDiactivation>() && other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName == "")
        {
            particleSystemsSmokeList.Add(other.transform.GetChild(0).GetComponent<ParticleSystem>());
            other.transform.GetChild(0).GetComponent<ParticleSystem>().trigger.SetCollider(0, flamezone);
            other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName = this.name;
        }

        if (other.GetComponent<BoxCollider>() && other.GetComponent<MouseDrag>()&&!dragObject.Contains(other.gameObject)&& other.GetComponent<MouseDrag>().zoneName == "")
        {
            dragObject.Add(other.gameObject);
            other.GetComponent<MouseDrag>().zoneName = name;
        }

        //foreach (GameObject percent in fireObject)
        //{
        //    percent.transform.GetComponent<FireActivationDiactivation>().partPercent = 1f / fireObject.Count;
        //} 
        this.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BoxCollider>()&& other.GetComponent<MouseDrag>())
        {
            foreach (GameObject objectDrag in dragObject)
            {
                objectDrag.transform.GetComponent<MouseDrag>().onEnter = true;
            }
        }
    }
}
