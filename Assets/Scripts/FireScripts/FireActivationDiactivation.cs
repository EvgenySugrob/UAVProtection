using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireActivationDiactivation : MonoBehaviour
{
    public string zoneName;

    public ParticleSystem fireParticleSystem;
    public FireParticleResize fireParticleResize;
    public BoxCollider boxCollider;

    [SerializeField] float colliderMaxSizeXZ = 5f;
    [SerializeField] float speed = 1f;

    private bool colliderISResize;

    private Vector3 startSizeFireCollider;

    public float  partPercent;
    public ZoneBurnPercentage zoneBurnPercentage;

    public bool fireIsActive;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        startSizeFireCollider = boxCollider.size;
        fireParticleResize = transform.GetChild(0).GetComponent<FireParticleResize>();
        fireParticleSystem = GetComponentInChildren<ParticleSystem>();


    }
    private void Update()
    {
        if (colliderISResize)
        {
            if (boxCollider.size.x < colliderMaxSizeXZ)
            {
                FireColliderResize();
            }
            else
            {
                colliderISResize = false;
            }
        }
    }

    public void FireActivation()
    {
        fireIsActive = true;



        if (zoneName == zoneBurnPercentage.zoneNamePercent)
        {
            Debug.Log(zoneName + " " + partPercent + " " + transform.name);
            zoneBurnPercentage.InterestAccumulation(partPercent);
        }


        
        StartCoroutine(WaitFireActivation(1f));
        StartCoroutine(WaitFireColliderResize(6f));
    }

    [System.Obsolete]
    public void FireDiactivation()
    {
        fireIsActive=false;
        colliderISResize = false;
        fireParticleSystem.Stop();
        fireParticleResize.FireSizeDown();
        fireParticleResize.FireGrowDown();
        boxCollider.size = startSizeFireCollider;
        StopAllCoroutines();

        if (zoneName == zoneBurnPercentage.zoneNamePercent)
        {
            zoneBurnPercentage.percent = 0;
            zoneBurnPercentage.totalZonePercent = 0;
        }
    }

    IEnumerator WaitFireActivation(float time)
    {
        yield return new WaitForSeconds(time);
        fireParticleSystem.Play();
        fireParticleResize.FireSizeUp();

    }
    IEnumerator WaitFireColliderResize(float time)
    {
        yield return new WaitForSeconds(time);
        FireColliderResizeBoolChange();
        fireParticleResize.FireGrowUpBoolChange();
    }
    public void FireColliderResizeBoolChange()
    {
        colliderISResize = true;
    }
    public void FireColliderResize()
    {
        boxCollider.size = new Vector3(Mathf.Lerp(boxCollider.size.x,colliderMaxSizeXZ, speed * Time.deltaTime),boxCollider.size.y,
            Mathf.Lerp(boxCollider.size.z,colliderMaxSizeXZ,speed*Time.deltaTime));
    }

    public void SetZoneName(string name)
    {
        zoneName = name;
    }
}
