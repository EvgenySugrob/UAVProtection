using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShowInventory : MonoBehaviour
{
    public GameObject inventoryPanel, checkResultBt;
    public RectTransform inventoryPanelStart,inventoryPanelClose, resultBtStart, resultBtEnd;

    public float speed = 3f;

    public bool isOpened;
    public bool isMove;
    public bool inventoryMove;
    public bool pressedTab;

    public FirstPersonController fpsControler;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isMove)
                pressedTab = true;
            isMove = true;
        }
        if (pressedTab)
            if (Vector3.Distance(inventoryPanel.transform.position, inventoryPanelClose.transform.position) > 1f && inventoryMove ||
            Vector3.Distance(inventoryPanel.transform.position, inventoryPanelStart.transform.position) > 1f && !inventoryMove)
            {
                pressedTab = !pressedTab;
                InventoryPanelOnOff();
            }
        if (isMove)
        {
            if (Vector3.Distance(inventoryPanel.transform.position, inventoryPanelClose.transform.position) > 1f && inventoryMove)
            {
                inventoryPanel.GetComponent<RectTransform>().transform.position = Vector3.Lerp(inventoryPanel.transform.position, inventoryPanelClose.transform.position, speed * Time.deltaTime);
                checkResultBt.GetComponent<RectTransform>().transform.position = Vector3.Lerp(checkResultBt.transform.position, resultBtEnd.transform.position, speed * Time.deltaTime);
                return;
            }
            else if (Vector3.Distance(inventoryPanel.transform.position, inventoryPanelClose.transform.position) < 1f && inventoryMove)
            {
                inventoryMove = false;
                isMove = false;
            }
            if (Vector3.Distance(inventoryPanel.transform.position, inventoryPanelStart.transform.position) > 1f && !inventoryMove)
            {
                inventoryPanel.GetComponent<RectTransform>().transform.position = Vector3.Lerp(inventoryPanel.transform.position, inventoryPanelStart.transform.position, speed * Time.deltaTime);
                checkResultBt.GetComponent<RectTransform>().transform.position = Vector3.Lerp(checkResultBt.transform.position, resultBtStart.transform.position, speed * Time.deltaTime);
                return;
            }
            else if (Vector3.Distance(inventoryPanel.transform.position, inventoryPanelStart.transform.position) < 1f && !inventoryMove)
            {
                inventoryMove = true;
                isMove =false;
            }
            
        }
        
    }

    public void CheckPanelReturnOnStart()
    {
        checkResultBt.GetComponent<RectTransform>().transform.position = resultBtEnd.transform.position;
    }

    public void InventoryPanelOnOff()
    {
        if (!isOpened)
        {
            //inventoryPanel.SetActive(true);
            isOpened = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            fpsControler.inventoryIsOpen = true;
        }
        else
        {
            //inventoryPanel.SetActive(false);
            isOpened = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            fpsControler.inventoryIsOpen = false;
        }

    }
}
