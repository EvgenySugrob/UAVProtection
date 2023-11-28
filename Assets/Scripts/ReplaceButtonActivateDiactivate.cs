using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceButtonActivateDiactivate : MonoBehaviour
{
    public GameObject replaceButtonOn, replaceButtonOff;
    public CameraSwitchPositionOnTop cameraSwitchPositionOnTop;

    [SerializeField] List<GameObject> objectToBurn;
    [SerializeField] List<GameObject> addObjectToBurn;

    OpenDoor[] needOffColliders;
    OpenAndCloseBlinds[] collidersOff;

    public GameObject[] activeButtonForJob;
    public ShowInventory showInventory;

    private void Start()
    {
        var burnObject = FindObjectsOfType<ObjectsToBurn>();
        for (int i = 0; i < burnObject.Length; i++)
        {
            objectToBurn.Add(burnObject[i].gameObject);
        }
        var addBurnObject = FindObjectsOfType<AdditionalObjectToBurn>();
        for (int i = 0; i < addBurnObject.Length; i++)
        {
            addObjectToBurn.Add(addBurnObject[i].gameObject);
        }
        replaceButtonOff.GetComponent<ButtonManager>().enabled = false;
        //replaceButtonOff.GetComponent<Button>().interactable = false;  //интерактивность новых кнопок
        StartCoroutine(ReplaceFix(0.5f));
    }
    public void ReplaceButtonState(bool stateOnBt, bool stateOffBt)
    {
        replaceButtonOff.SetActive(stateOffBt);

        foreach (GameObject burnObject in objectToBurn)
        {
            burnObject.transform.GetComponent<ObjectsToBurn>().isActiv = stateOnBt;
        }
        foreach (GameObject addBurnObject in addObjectToBurn)
        {
            addBurnObject.transform.GetComponent<AdditionalObjectToBurn>().isActiv = stateOnBt;
        }
    }

    public void ReplaceButtonActivate()
    {
        ReplaceButtonState(false, true);
        needOffColliders = FindObjectsOfType<OpenDoor>();
        foreach (OpenDoor collider in needOffColliders)
        {
            collider.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        collidersOff = FindObjectsOfType<OpenAndCloseBlinds>();
        foreach (OpenAndCloseBlinds collider in collidersOff)
        {
            collider.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        cameraSwitchPositionOnTop.ReplaceOn();
    }

    //Apply changes after placing objects. Executed by clicking on ReplaceObjectsOffBt
    public void ReplaceButtonDeacivate()
    {
        ReplaceButtonState(true, false);
        foreach (OpenDoor collider in needOffColliders)
        {
            collider.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        foreach (OpenAndCloseBlinds collider in collidersOff)
        {
            collider.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        cameraSwitchPositionOnTop.ReplaceOff();

        showInventory.CheckPanelReturnOnStart();
        for (int i = 0; i < activeButtonForJob.Length; i++)
        {
            activeButtonForJob[i].SetActive(true);
        }
    }

    IEnumerator ReplaceFix(float time)
    {
        yield return new WaitForSeconds(time);
        ReplaceButtonActivate();
    }
}
