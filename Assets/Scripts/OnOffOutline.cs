using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffOutline : MonoBehaviour
{
    public Outline outline;
    [SerializeField] bool needSeeChild = true;

    private void Start()
    {
        if (needSeeChild)
            outline = transform.GetChild(0).GetComponent<Outline>();
        else
            outline = gameObject.GetComponent<Outline>();
    }

    private void OnMouseOver()
    {
        if (Camera.main.transform.parent.GetComponent<CharacterController>())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Vector3.Distance(gameObject.transform.position, Camera.main.transform.position)));
            if (Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) <= 6.1f)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        } 
    }

    private void OnMouseExit()
    {
        if (Camera.main.transform.parent.GetComponent<CharacterController>())
            outline.enabled = false;
    }
}
