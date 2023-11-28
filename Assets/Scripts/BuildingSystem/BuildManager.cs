using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public GameObject prefab;
	[HideInInspector]
    public GameObject created;
    public Vector3 rot;
    Quaternion trot;
    public Camera playerCamera;
    public float distanceRay=3f;

    public ShowInventory showInventory;
    public Builds builds;
    public List<GameObject> setZoneParent;
    public List<GameObject> setZoneSmokeParent;
    public List<GameObject> setInZone;


    //Filling in the lists of sensor placement zones
    private void Start()
    {
        var allZone = FindObjectsOfType<SetZoneParent>();
        var allSmokeZone = FindObjectsOfType<SetZoneSmokeParent>();
        var allZoneMove = FindObjectsOfType<SetInZones>();

        for (int i = 0; i < allZone.Length; i++)
        {
            setZoneParent.Add(allZone[i].gameObject);
        }
        for (int i = 0; i < allSmokeZone.Length; i++)
        {
            setZoneSmokeParent.Add(allSmokeZone[i].gameObject);
        }
        for (int i = 0; i < allZoneMove.Length; i++)
        {
            setInZone.Add(allZoneMove[i].gameObject);
        }
    }

    void Update()
    {
        if (!showInventory.isOpened)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            /*Checking for the absence of a sensor in the hand and the presence of the DeleteObj component on the object being deleted*/

            if (Physics.Raycast(ray, out hit, distanceRay))
            {
                if (prefab == null && hit.transform.GetComponent<DeletObj>())
                {
                    /*By pressing RMB, search by sheets and delete*/
                    if (Input.GetMouseButtonDown(1))
                    {
                        foreach (GameObject zone in setZoneParent)
                        {
                            foreach (GameObject sensor in zone.GetComponent<SetZoneParent>().zoneObject) //Fire detector
                            {
                                if (sensor == hit.transform.gameObject)
                                {
                                    zone.GetComponent<SetZoneParent>().zoneObject.Remove(sensor);
                                    Destroy(hit.transform.gameObject);
                                    break;
                                }
                            }
                        }
                        foreach (GameObject zone in setZoneSmokeParent)
                        {
                            foreach (GameObject sensorSmoke in zone.GetComponent<SetZoneSmokeParent>().smokeZoneObject) //Smoke detector
                            {
                                if (sensorSmoke == hit.transform.gameObject)
                                {
                                    zone.GetComponent<SetZoneSmokeParent>().smokeZoneObject.Remove(sensorSmoke);
                                    Destroy(hit.transform.gameObject);
                                    break;
                                }
                            }
                        }
                        foreach (GameObject zoneMove in setInZone)
                        {
                            foreach (GameObject sensorMove in zoneMove.GetComponent<SetInZones>().zoneObjects) //Motion Sensor
                            {
                                if (sensorMove == hit.transform.gameObject)
                                {
                                    zoneMove.GetComponent<SetInZones>().zoneObjects.Remove(sensorMove);
                                    Destroy(hit.transform.gameObject);
                                    break;
                                }
                            }
                        }
                        foreach (GameObject sensor in builds.sensOpenDoor)
                        {
                            if (sensor == hit.transform.gameObject)
                            {
                                sensor.GetComponent<SetOpenDoorSensor>().isSetSensor = false;
                                sensor.GetComponent<SetOpenDoorSensor>().connectedSensor.GetComponent<SetOpenDoorSensor>().isSetSensor = false;
                                sensor.GetComponent<SetOpenDoorSensor>().connectedSensor.SetActive(false);
                                sensor.GetComponent<SetOpenDoorSensor>().connectedSensor.GetComponent<SetOpenDoorSensor>().RemoveProtectedZone();
                                sensor.gameObject.GetComponent<SetOpenDoorSensor>().RemoveProtectedZone();
                                sensor.SetActive(false);
                            }
                        }
                    }
                }

                /*If there is a clone of the selected prefab, then by pressing RMB, the projection is removed and the hand is released*/
                if (created != null)
                {
                    trot = Quaternion.Lerp(created.transform.rotation, Quaternion.Euler(rot), 0.2f);

                    if (Input.GetKeyDown(KeyCode.Mouse1) && !IsMouseOverUI())
                    {
                        Destroy(created.gameObject);
                        prefab = null;
                    }
                }

                /*Rotate and position an object*/
                if (Physics.Raycast(ray, out hit, distanceRay))
                {
                    var pos = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                    if (prefab != null)
                    {
                        if (created == null)
                        {
                            created = Instantiate(prefab, pos, Quaternion.Euler(rot));
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.R))
                            {
                                rot += new Vector3(0, 45f, 0);
                            }
                            if (Input.GetKeyDown(KeyCode.Mouse0) && !IsMouseOverUI())
                            {
                                if (created.GetComponent<PrefabBuild>().Place(pos, rot))
                                {
                                    created = null;
                                    return;
                                }

                            }
                            created.transform.rotation = trot;
                            created.transform.position = Vector3.Lerp(created.transform.position, pos, 10f * Time.deltaTime);
                        }
                    }
                }
            }
        }
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
