using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class SaveData //object parametrs, which would be save for returning on scene
{
    public string zoneName;
    public int numPrefab;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
}

public class SaveSystem : MonoBehaviour
{
    public string savedZone;
    public int savedNumPrefab;
    public float savedPosX, savedPosY, savedPosZ;
    public float savedRotX, savedRotY, savedRotZ;
    GameObject zoneSpawn, prefabSpawn;



    [SerializeField] GameObject[] zones, prefabs, notZones;
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        List<SaveData> finData = new List<SaveData>();

        foreach (GameObject zone in zones) //save all sensors, which are childs of zones
        {
            foreach (Transform child in zone.transform)
            {
                SaveData data = new SaveData();
                savedZone = zone.name;
                savedNumPrefab = child.gameObject.GetComponent<ConnectPrefabAndObject>().numPrefab;
                savedPosX = child.transform.localPosition.x;
                savedPosY = child.transform.localPosition.y;
                savedPosZ = child.transform.localPosition.z;
                savedRotX = child.transform.localEulerAngles.x;
                savedRotY = child.transform.localEulerAngles.y;
                savedRotZ = child.transform.localEulerAngles.z;


                data.zoneName = savedZone;
                data.numPrefab = savedNumPrefab;
                data.posX = savedPosX;
                data.posY = savedPosY;
                data.posZ = savedPosZ;
                data.rotX = savedRotX;
                data.rotY = savedRotY;
                data.rotZ = savedRotZ;
                finData.Add(data); 
            }
        }
        //save all open door sensors
        foreach (GameObject notZone in notZones)
        {
            foreach (Transform child in notZone.transform)
            {
                if (child.gameObject.GetComponent<SetOpenDoorSensor>() && child.gameObject.activeSelf)
                {
                    if (child.gameObject.GetComponent<SetOpenDoorSensor>().isSetSensor)
                    {
                        SaveData data = new SaveData();
                        savedZone = notZone.name;
                        savedNumPrefab = 99;
                        savedPosX = child.transform.localPosition.x;
                        savedPosY = child.transform.localPosition.y;
                        savedPosZ = child.transform.localPosition.z;
                        savedRotX = child.transform.localEulerAngles.x;
                        savedRotY = child.transform.localEulerAngles.y;
                        savedRotZ = child.transform.localEulerAngles.z;


                        data.zoneName = savedZone;
                        data.numPrefab = savedNumPrefab;
                        data.posX = savedPosX;
                        data.posY = savedPosY;
                        data.posZ = savedPosZ;
                        data.rotX = savedRotX;
                        data.rotY = savedRotY;
                        data.rotZ = savedRotZ;
                        finData.Add(data);
                    }
                }
            }
        }
        bf.Serialize(file, finData); //writing in file

        file.Close();
        Debug.Log("Game data saved! " + Application.persistentDataPath);
    }
    public void Load()
    {
        foreach (GameObject zone in zones) //clear all zones particleSystems
        {
            zone.GetComponent<CombustionZone>().enabled = true;
            zone.GetComponent<CombustionZone>().particleSystemsList.Clear();
            zone.GetComponent<CombustionZone>().particleSystemsSmokeList.Clear();
        }

        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            List<SaveData> data = (List<SaveData>)bf.Deserialize(file);
            file.Close();
            foreach (GameObject zone in zones) //remove all childs in zones
            {
                foreach (Transform child in zone.transform)
                Destroy(child.gameObject);
            }
            foreach (GameObject notZone in notZones) //remove all open door sensors
            {
                foreach (Transform child in notZone.transform)
                {
                    if (child.gameObject.GetComponent<SetOpenDoorSensor>())
                    {
                        child.gameObject.SetActive(false);
                        child.gameObject.GetComponent<SetOpenDoorSensor>().isSetSensor = false;
                        child.gameObject.GetComponent<SetOpenDoorSensor>().ChangeMaterialOnAwake();
                        child.gameObject.GetComponent<SetOpenDoorSensor>().RemoveProtectedZone();
                    }
                }
            }
            foreach (SaveData objectLoad in data)
            {
                bool inZone = false;
                savedZone = objectLoad.zoneName;
                savedNumPrefab = objectLoad.numPrefab;
                savedPosX = objectLoad.posX;
                savedPosY = objectLoad.posY;
                savedPosZ = objectLoad.posZ;
                savedRotX = objectLoad.rotX;
                savedRotY = objectLoad.rotY;
                savedRotZ = objectLoad.rotZ;


                foreach (GameObject zone in zones) //check zone of loading sensor
                {
                    if (zone.name == savedZone)
                    {
                        zoneSpawn = zone;
                        inZone = true;
                        break;
                    }
                }
                if (inZone) //return sensor, which was zone child
                {
                    prefabSpawn = prefabs[savedNumPrefab];
                    GameObject spawnObject = Instantiate(prefabSpawn, zoneSpawn.transform);
                    spawnObject.transform.localPosition = new Vector3(savedPosX, savedPosY, savedPosZ);
                    spawnObject.transform.localEulerAngles = new Vector3(savedRotX, savedRotY, savedRotZ);
                    spawnObject.AddComponent<DeletObj>();
                    Destroy(spawnObject.GetComponent<PrefabBuild>());
                    if (savedNumPrefab != 5)
                        spawnObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                else //return open door sensor
                {
                    GameObject notZone = GameObject.Find(savedZone);
                    foreach (Transform child in notZone.transform)
                    {
                        if (child.gameObject.GetComponent<SetOpenDoorSensor>())
                        {
                            child.gameObject.SetActive(true);
                            child.gameObject.GetComponent<SetOpenDoorSensor>().ForLoad();
                        }
                    }   
                }
            }
            foreach (GameObject zone in zones)
            {
                zone.GetComponent<CombustionZone>().enabled = false;
            }
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }
}
