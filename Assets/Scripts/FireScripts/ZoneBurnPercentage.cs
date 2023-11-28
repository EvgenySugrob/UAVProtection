using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Percentage of burning of a certain zone. The script must be located on each zone in which there are object of ignition

public class ZoneBurnPercentage : MonoBehaviour
{
    public string zoneNamePercent;

    [SerializeField] float stepGrowUpPercentBar = 0.002f;

    public float percent;
    public float totalZonePercent;
    public float minPercent = 0f;
    public float maxPercent = 1f;

    public List<GameObject> fireObject;

    public Image fillProgress;
    public TMP_Text textProcent;

    public bool stopPercent;

    private void Start()
    {
        zoneNamePercent = name;
    }

    //For each fire object, a percentage is calculated, which is displayed when a fire occurs in a certain zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<FireActivationDiactivation>() && other.GetComponent<FireActivationDiactivation>().zoneName == zoneNamePercent)
        {
            fireObject.Add(other.transform.GetComponent<FireActivationDiactivation>().gameObject);
        }
        foreach (GameObject percent in fireObject)
        {
            percent.transform.GetComponent<FireActivationDiactivation>().partPercent = 1f / fireObject.Count;
            percent.transform.GetComponent<FireActivationDiactivation>().zoneBurnPercentage = transform.GetComponent<ZoneBurnPercentage>();
        }
    }

    public void InterestAccumulation(float percentTransfer)
    {
         percent += percentTransfer;

    }

    //Smooth accumulation of the progress bar for each zone
    private void FixedUpdate()
    {
        if (!stopPercent)
        {
            if (totalZonePercent < percent)
            {
                totalZonePercent += stepGrowUpPercentBar;
                fillProgress.fillAmount = totalZonePercent;
                textProcent.text = Mathf.Round(totalZonePercent * 100f).ToString() + "%";
            }
        }
    }
}
