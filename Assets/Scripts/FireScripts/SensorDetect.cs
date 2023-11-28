using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorDetect : MonoBehaviour
{
    public string zoneName;

    public Renderer sensorZoneRender;
    public Material alarm;
    private Material notAlarm;
    [SerializeField] Light spotLightSensor;

    private bool corutineIsStop = true;
    public ShowerOnOff showerOnOff;
    public CameraSwitchPositionOnTop cameraSwitchPositionOnTop;

    private void Start()
    {
        notAlarm = sensorZoneRender.GetComponent<Renderer>().material;
        spotLightSensor = transform.GetChild(0).GetComponent<Light>();
        showerOnOff = FindObjectOfType<ShowerOnOff>();
        cameraSwitchPositionOnTop = FindObjectOfType<CameraSwitchPositionOnTop>();
    }
    //Detection of active ignition sources.
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<FireActivationDiactivation>() &&
           other.GetComponent<FireActivationDiactivation>().zoneName == zoneName)
        {
            if (other.GetComponentInChildren<ParticleSystem>().isPlaying)
            {
                sensorZoneRender.material = alarm;
                spotLightSensor.color = Color.red;
                cameraSwitchPositionOnTop.StopProgressBar();
    
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<FireActivationDiactivation>() &&
            other.GetComponent<FireActivationDiactivation>().zoneName == zoneName)
        {
            
            if(other.GetComponentInChildren<ParticleSystem>().isPlaying)
            {
                sensorZoneRender.material = alarm;
                spotLightSensor.color = Color.red;
                if (spotLightSensor.color == Color.red)
                {
                    cameraSwitchPositionOnTop.StopProgressBar();
                    showerOnOff.ShowerOn();
                    StartCoroutine(WaitShowerStart(3f));
                }
            }
            else if (other.GetComponentInChildren<ParticleSystem>().isStopped && corutineIsStop)
            {
                sensorZoneRender.material = notAlarm;
                spotLightSensor.color = Color.green;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FireActivationDiactivation>())
            if (other.GetComponent<BoxCollider>() &&
                other.GetComponent<FireActivationDiactivation>().zoneName == zoneName || other.GetComponent<FireActivationDiactivation>().zoneName != zoneName)
            {
                if ((other.GetComponentInChildren<ParticleSystem>().isStopped || other.GetComponentInChildren<ParticleSystem>().isPlaying) && 
                    other.GetComponent<FireActivationDiactivation>().zoneName == zoneName)
                {
                    sensorZoneRender.material = notAlarm;
                    spotLightSensor.color = Color.green;
                }
            }
    }

    public void SetName(string name)
    {
        zoneName = name;
    }

    IEnumerator WaitShowerStart(float time)
    {
        corutineIsStop = false;
        yield return new WaitForSeconds(time);
        cameraSwitchPositionOnTop.ShowerDeactivation();
        corutineIsStop = true;
    }
}
