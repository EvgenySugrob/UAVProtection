using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTypeSensor : MonoBehaviour
{
    public GameObject inventoryPozar;
    public GameObject inventorySecurity;

    public Button pozBt, ohrnBt;

    private void Start()
    {
        pozBt.interactable = false;
        inventoryPozar.SetActive(true);
        inventorySecurity.SetActive(false);
    }

    public void SwitchOnPozar()
    {
        pozBt.interactable = false;
        ohrnBt.interactable = true;
        inventoryPozar.SetActive(true);
        inventorySecurity.SetActive(false);
    }

    public void SwitchOnSecurity()
    {
        pozBt.interactable = true;
        ohrnBt.interactable = false;
        inventoryPozar.SetActive(false);
        inventorySecurity.SetActive(true);
    }
}
