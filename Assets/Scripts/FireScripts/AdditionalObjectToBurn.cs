using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalObjectToBurn : MonoBehaviour
{
    public bool isActiv;
    private void Start()
    {
        isActiv = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isActiv)
        {
            if (other.GetComponent<ObjectsToBurn>())
            {
                other.transform.GetChild(0).GetComponent<SmokeDioxidActivationDiactivation>().SmokeDioxidActivation();

                if (!other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().fireIsActive)
                {
                    other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().FireActivation();
                }

                //if (!other.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().isPlaying)
                //{
                //    other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().FireActivation();
                //}
            }
            else if (other.GetComponent<AdditionalObjectToBurn>())
            {
                if (!other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().fireIsActive)
                {
                    other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().FireActivation();
                }

                //if (!other.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().isPlaying)
                //{
                //    other.transform.GetChild(1).GetComponent<FireActivationDiactivation>().FireActivation();
                //}
            }
        }
        
    }
}
