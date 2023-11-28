using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDioxidActivationDiactivation : MonoBehaviour
{
    public string zoneName;

    public ParticleSystem[] smokeDioxidParticleSystem = new ParticleSystem[2];
    public SmokeParticleResize smokeParticleResize;
    public BoxCollider boxCollider;

    [SerializeField] float colliderSizeY = 4f;
    [SerializeField] float colliderCenterY = 1.2f;
    [SerializeField] float speed = 1f;

    private bool colliderISResize;

    private Vector3 startColliderSize;
    private Vector3 startColliderCenter;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        startColliderSize = boxCollider.size;
        startColliderCenter = boxCollider.center;
        smokeParticleResize = transform.GetChild(0).GetComponent<SmokeParticleResize>();
        for (int i = 0; i < smokeDioxidParticleSystem.Length; i++)
        {
            smokeDioxidParticleSystem[i] = transform.GetChild(i).GetComponent<ParticleSystem>();

        }
    }

    public void Update()
    {
        if (colliderISResize)
        {
            if (boxCollider.size.y < colliderSizeY && boxCollider.center.y < colliderCenterY)
            {
                SmokeColliderResize();
            }
            else
            {
                colliderISResize = false;
            }
        }
    }

    public void SmokeDioxidActivation()
    {
        for (int i = 0; i < smokeDioxidParticleSystem.Length; i++)
        {
            smokeDioxidParticleSystem[i].Play();
        }
        smokeParticleResize.SmokeSizeUp();
        SmokeColliderResizeBoolChange();

    }

    [System.Obsolete]
    public void SmokeDioxidDiactivation()
    {
        for (int i = 0; i < smokeDioxidParticleSystem.Length; i++)
        {
            smokeDioxidParticleSystem[i].Stop();
        }
        colliderISResize = false;
        smokeParticleResize.SmokeResizeDown();
        SmokeColliderDownSize();
    }

    public void SmokeColliderResizeBoolChange()
    {
        colliderISResize = true;
    }

    public void SmokeColliderResize()
    {
        boxCollider.size = new Vector3(boxCollider.size.x, Mathf.Lerp(boxCollider.size.y, colliderSizeY, speed * Time.deltaTime), boxCollider.size.z);
        boxCollider.center = new Vector3(boxCollider.center.x, Mathf.Lerp(boxCollider.center.y, colliderCenterY, speed * Time.deltaTime), boxCollider.center.z);
    }

    public void SmokeColliderDownSize()
    {
        boxCollider.size = startColliderSize;
        boxCollider.center = startColliderCenter;
    }
}
