using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
//Script should be attached on DoorSens
public class SetOpenDoorSensor : MonoBehaviour
{
    public GameObject connectedSensor;
    [SerializeField] NavMeshModifier protectedZone;
    [SerializeField] Material whitePlastic, setMaterial;
    public bool isSetSensor = false;
    [SerializeField] float minY, maxY;


    public void ChangeMaterialOnAwake()
    {
        gameObject.GetComponent<Renderer>().material = setMaterial;
        connectedSensor.GetComponent<Renderer>().material = setMaterial;
    }
    private void OnMouseDown() //setting sensor
    {
        if (!isSetSensor)
        {
            gameObject.GetComponent<Renderer>().material = whitePlastic;
            connectedSensor.GetComponent<Renderer>().material = whitePlastic;
            isSetSensor = connectedSensor.GetComponent<SetOpenDoorSensor>().isSetSensor = true;
            protectedZone.enabled = true;
            SetNeedBrokeInConnectedSens();
            connectedSensor.GetComponent<SetOpenDoorSensor>().SetNeedBrokeInConnectedSens();

        }
    }
    public void ForLoad() //set parametrs then loading object which have this script
    {
        gameObject.GetComponent<Renderer>().material = whitePlastic;
        connectedSensor.GetComponent<Renderer>().material = whitePlastic;
        isSetSensor = connectedSensor.GetComponent<SetOpenDoorSensor>().isSetSensor = true;
        protectedZone.enabled = true;
        SetNeedBrokeInConnectedSens();
        connectedSensor.GetComponent<SetOpenDoorSensor>().SetNeedBrokeInConnectedSens();
    }
    public void RemoveProtectedZone()
    {
        protectedZone.enabled = false;
    }
    public void SetNeedBrokeInConnectedSens() //if open door sensor is set thief takes command that need break glass
    {
        if (transform.parent.gameObject.GetComponent<OpenDoor>())
            if (transform.parent.gameObject.GetComponent<OpenDoor>().isWindowDoor)
                transform.parent.gameObject.GetComponent<OpenDoor>().needBrokeGlass = true;
    }
    private void OnMouseOver()
    {
        if (!isSetSensor)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                Vector3.Distance(gameObject.transform.position, Camera.main.transform.position)));
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Clamp(mousePos.y, minY, maxY), gameObject.transform.position.z);
            connectedSensor.transform.position = new Vector3(connectedSensor.transform.position.x, Mathf.Clamp(mousePos.y, minY, maxY), connectedSensor.transform.position.z);
        }
    }
}
