using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Script should be attached on all objects, what can be stealed
public class SteelScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //if thief in object area he's steal this object and goes further 
    {
        if (other.gameObject.GetComponent<NavMeshAgent>() && gameObject == other.gameObject.GetComponent<Walk>().pointsSteel[other.gameObject.GetComponent<Walk>().steelNum].gameObject)
        {
            other.gameObject.GetComponent<Walk>().isFinish = false;

            if (other.gameObject.GetComponent<Walk>().pointNum + 1 == other.gameObject.GetComponent<Walk>().pointsSteel.Count)
                other.gameObject.GetComponent<Walk>().isFinish = true;
            Camera.main.GetComponent<CheckYourWork>().AddCount();
            if (!other.gameObject.GetComponent<Walk>().isFinish)
            {
                other.gameObject.GetComponent<Walk>().pointNum++;
                other.gameObject.GetComponent<Walk>().MovingThief();
                gameObject.SetActive(false);
            }
            else //this object is last in steal list
            {
                Camera.main.GetComponent<CheckYourWork>().TimerOff();
                gameObject.SetActive(false);
            }
        }
    }
}
