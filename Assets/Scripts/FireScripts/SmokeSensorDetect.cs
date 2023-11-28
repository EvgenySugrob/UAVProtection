using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSensorDetect : MonoBehaviour
{
    public string zoneName;

    public Renderer sensorZoneRender;
    public Material alarm;
    private Material notAlarm;
    [SerializeField] Light spotLightSensor;

    public ShowerOnOff showerOnOff;
    public CameraSwitchPositionOnTop cameraSwitchPositionOnTop;

    private void Start()
    {
        notAlarm = sensorZoneRender.GetComponent<Renderer>().material;
        spotLightSensor = transform.GetChild(0).GetComponent<Light>();

        showerOnOff = FindObjectOfType<ShowerOnOff>();
        cameraSwitchPositionOnTop = FindObjectOfType<CameraSwitchPositionOnTop>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<SmokeDioxidActivationDiactivation>() &&
            other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName == zoneName)
        {
            if (other.GetComponentInChildren<ParticleSystem>().isPlaying)
            {
                sensorZoneRender.material = alarm;
                spotLightSensor.color = Color.cyan;
                if (spotLightSensor.color == Color.cyan)
                {
                    cameraSwitchPositionOnTop.StopProgressBar();
                    showerOnOff.ShowerOn();
                    StartCoroutine(WaitShowerStart(4f));
                }
            }
            if (other.GetComponentInChildren<ParticleSystem>().isStopped)
            {
                sensorZoneRender.material = notAlarm;
                spotLightSensor.color = Color.yellow;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BoxCollider>() && other.GetComponent<SmokeDioxidActivationDiactivation>() &&
            other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName == zoneName || other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName != zoneName)
        {
            if (other.GetComponentInChildren<ParticleSystem>().isStopped || other.GetComponentInChildren<ParticleSystem>().isPlaying &&
            other.GetComponent<SmokeDioxidActivationDiactivation>().zoneName == zoneName)
            {
                spotLightSensor.color = Color.yellow;
                sensorZoneRender.material = notAlarm;
            }
        }
    }
    IEnumerator WaitShowerStart(float time)
    {
        yield return new WaitForSeconds(time);
        cameraSwitchPositionOnTop.ShowerDeactivation();
    }
}
