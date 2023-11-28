using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builds : MonoBehaviour
{
    public GameObject[] objects;
    public List<GameObject> sensOpenDoor;
    public ShowInventory showInventory;
    public GameObject crosshair;

    private void Start()
    {
        var allOpenDoorSens = FindObjectsOfType<SetOpenDoorSensor>();
        foreach (var sensor in allOpenDoorSens)
        {
            sensOpenDoor.Add(sensor.gameObject);
            sensor.gameObject.SetActive(false);
        }
    }

    public void SetPrefab(int id)
    {
        HideSensorOpenDoor();
        if (FindObjectOfType<BuildManager>().created != null)
        {
            Destroy(FindObjectOfType<BuildManager>().created);
        }
        FindObjectOfType<BuildManager>().prefab = objects[id];
        showInventory.pressedTab = true;
        showInventory.isMove = true;
    }
    public void PlacingSensorOpenDoor()
    {
        foreach (GameObject sensor in sensOpenDoor)
            if (!sensor.GetComponent<SetOpenDoorSensor>().isSetSensor)
            {
                crosshair.SetActive(true);
                sensor.SetActive(true);
                sensor.GetComponent<SetOpenDoorSensor>().ChangeMaterialOnAwake();
            }
        if (FindObjectOfType<BuildManager>().created != null)
        {
            Destroy(FindObjectOfType<BuildManager>().created);
            
        }
        FindObjectOfType<BuildManager>().prefab = null;
        showInventory.pressedTab = true;
        showInventory.isMove = true;
    }
    public void HideSensorOpenDoor()
    {
        foreach (GameObject sensor in sensOpenDoor)
            if (!sensor.GetComponent<SetOpenDoorSensor>().isSetSensor)
            {
                crosshair.SetActive(false);
                sensor.SetActive(false);
            }
    }
}
