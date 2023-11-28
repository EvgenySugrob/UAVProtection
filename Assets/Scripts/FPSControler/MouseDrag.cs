using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 objectStartPosition;
    private Quaternion objectStartRotation;

    private Vector3 rotation;
    private Quaternion transformRotation;
    private Color startColor;
    private Outline outline;

    public bool dragIsOn,onEnter;

    public string zoneName;
    private void Start()
    {
        objectStartPosition = transform.position;
        objectStartRotation = transform.rotation;
        outline = GetComponent<Outline>();
        startColor = outline.OutlineColor;
    }

    private void OnMouseOver()
    {
        if (dragIsOn)
        {
            outline.enabled = true;
        }
        
    }
    private void OnMouseExit()
    {
        if (dragIsOn)
        {
            outline.enabled = false;
        }
        
    }
    private void OnMouseDown()
    {
        if (dragIsOn)
        {
            outline.OutlineColor = Color.green;
            objectStartPosition = transform.position;
            objectStartRotation = transform.rotation;
        }
    }

    //Moving and moving by pressing LMB on the subject of interaction
    private void OnMouseDrag()
    {
        if (dragIsOn)
        {

            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
            transformRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.2f);

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotation += new Vector3(0, 90f, 0);
            }
            transform.rotation = transformRotation;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<MeshCollider>() && other.GetComponent<CorrectAttachmentLocation>())
            ||(other.GetComponent<BoxCollider>() && other.GetComponent<MouseDrag>()) || (zoneName!=other.gameObject.name && other.GetComponent<CombustionZone>() && zoneName != "") 
            || other.GetComponent<OpenDoor>())
        {
            onEnter = true;
            outline.OutlineColor = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.GetComponent<BoxCollider>() && other.GetComponent<MouseDrag>()) /*|| (other.GetComponent<MeshCollider>() && other.GetComponent<CorrectAttachmentLocation>())*/)
        {
            onEnter = false;
            outline.OutlineColor = Color.green;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<SetInZones>())
        {
            if (zoneName != other.name)
            {
                onEnter = true;
                outline.OutlineColor = Color.red;
            }
        }
    }

    private void OnMouseUp()
    {
        outline.OutlineColor = startColor;
        if (onEnter)
        {
            transform.position = objectStartPosition;
            transform.rotation = objectStartRotation;
            onEnter = false;
        }
    }
}
