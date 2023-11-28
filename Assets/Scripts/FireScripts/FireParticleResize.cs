using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FireParticleResize : MonoBehaviour
{
    public ParticleSystem fire;
    public bool isResize;
    public bool isGrowUp;

    [SerializeField] float maxSize = 3f;
    [SerializeField] float minSize = 0f;
    [SerializeField] float maxRate = 5f;
    [SerializeField] float minRate = 1f;
    [SerializeField] float maxShapeSize = 3f;
    [SerializeField] float minShapeSize = 0.2f;
    [SerializeField] float speed=0.5f;

    private void Start()
    {
        fire = GetComponent<ParticleSystem>();
    }


    private void Update()
    {
        if (isResize)
        {
            if (fire.startSize<maxSize)
            {
                fire.startSize = Mathf.Lerp(fire.startSize, maxSize, Time.deltaTime);
            }
            else
            {
                isResize = false;
            }
            
        }
        if (isGrowUp)
        {
            if (fire.emissionRate < maxRate)
            {
                FireGrowUp();
            }
            else
            {
                isGrowUp = false;
            }
            
        }
    }

    public void FireSizeUp()
    {
        isResize = true;
    }

    [System.Obsolete]
    public void FireSizeDown()
    {
        isResize = false;
        fire.startSize = minSize;
    }
    public void FireGrowUpBoolChange()
    {
        isGrowUp = true;
    }

    [System.Obsolete]
    public void FireGrowDown()
    {
        isGrowUp = false;

        var fireShape = fire.shape;
        fireShape.radius = minShapeSize;
        fire.emissionRate = minRate;
    }

    [System.Obsolete]
    public void FireGrowUp()
    {
        var fireShape = fire.shape;
        fireShape.radius = Mathf.Lerp(fire.shape.radius,maxShapeSize,speed * Time.deltaTime);
        fire.emissionRate = Mathf.Lerp(fire.emissionRate, maxRate, speed * Time.deltaTime);
    }
}
