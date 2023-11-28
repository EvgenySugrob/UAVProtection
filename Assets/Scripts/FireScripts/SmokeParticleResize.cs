using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleResize : MonoBehaviour
{
    public ParticleSystem smoke;
    public bool isResize;

    [SerializeField] float maxSize = 5f;
    [SerializeField] float minSize = 0f;

    private void Start()
    {
        smoke = GetComponent<ParticleSystem>();
    }


    private void Update()
    {
        if (isResize)
        {
            if (smoke.startSize<maxSize)
            {
                smoke.startSize = Mathf.Lerp(smoke.startSize, maxSize, Time.deltaTime);
            }
            else
            {
                isResize = false;
            }
        }
    }

    public void SmokeSizeUp()
    {
        isResize = true;
    }

    [System.Obsolete]
    public void SmokeResizeDown()
    {
        isResize=false;
        smoke.startSize = minSize;
    }
}
