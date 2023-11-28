using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionActivate : MonoBehaviour
{
    public ParticleSystem explosion;

    public bool isActive;

    private bool checkExpl;
    private void Start()
    {
        isActive = false;
        checkExpl = true;
        explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FireActivationDiactivation>())
        {
            isActive=true;
            if (isActive && checkExpl)
            {
                checkExpl=false;
                StartCoroutine(Bax(5f));
            }
        }  
    }

    IEnumerator Bax(float time)
    {
        yield return new WaitForSeconds(time);
        explosion.Play();
    }
}
