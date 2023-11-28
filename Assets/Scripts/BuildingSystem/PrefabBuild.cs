using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//The script is responsible for displaying the object that will be installed. Changes the mesh to the given material, and after setting returns the initial material of the prefab

public class PrefabBuild : MonoBehaviour
{
    Collider[] colliders;
    MeshFilter[] meshes;
    public List<Material> materials = new List<Material>();
    public Transform rayObject;
    public bool isCan;
    public Material canM,cantM;
    public float rayDistance=0.3f;

    [SerializeField] Light spotLightColorSet;
    private void Start()
    {
        spotLightColorSet = transform.GetChild(2).transform.GetChild(0).GetComponent<Light>();
        spotLightColorSet.enabled = false;

        colliders = GetComponentsInChildren<Collider>();
        meshes = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshes.Length; i++)
        {
            materials.Add(meshes[i].GetComponent<Renderer>().material);
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    //Raycast on the prefab checks if the object has a component that allows you to put a sensor on it
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayObject.transform.position, rayObject.transform.forward, out hit, rayDistance))
        {
            Debug.DrawRay(rayObject.transform.position, rayObject.forward* rayDistance, Color.green);
            if (!hit.transform.GetComponent<CorrectAttachmentLocation>())
            {
                isCan = false;
                
            }
            else
            { 
                isCan = true;
            }
        }
        else
        {
            isCan = false;
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].GetComponent<Renderer>().material = isCan ? canM : cantM;
            //spotLightColorSet.color = isCan ? Color.green : Color.red;
        }
    }

    //Installation of the sensor with the return of the original material to it
    public bool Place(Vector3 pos, Vector3 local)
    {   
       transform.position = pos;
       transform.localEulerAngles = local;
       transform.AddComponent<DeletObj>();
        if (isCan)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].GetComponent<Renderer>().material = materials[i];
                if (meshes[i].GetComponent<SensorTag>())
                {
                    meshes[i].GetComponent<Renderer>().enabled = false; 
                }
            }
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }
            spotLightColorSet.enabled = true;

        }
        if (isCan){
            Destroy(this);
        }
	    return isCan;
    }
}
