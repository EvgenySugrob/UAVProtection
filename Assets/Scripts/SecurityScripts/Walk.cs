using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.UI;
//Thief contrrol script, which should be attached on thief prefab
public class Walk : MonoBehaviour
{
    public int pointNum = 0, steelNum, maxChance = 1;
    public List<Transform> steelObjects;
    [SerializeField] NavMeshAgent myAgent;
    public List<GameObject> pointsSteel;
    public List<NavMeshModifier> navMeshModifierNeedReturn;
    public EscapeZone escapeZone;
    public string zoneName;
    public bool isFinish = false;

    private void Start()
    {
        foreach (Transform child in steelObjects)
        {
            if (child.gameObject.activeSelf)
            {
                pointsSteel.Add(child.gameObject);
            }
        }
        escapeZone = FindObjectOfType<EscapeZone>();
        steelNum = -1;
        MovingThief();
    }
    public void MovingThief()
    {
        
         int pointNumber = Random.Range(0, pointsSteel.Count);  //Branching with the choice of a random object to steal: if the object was not stolen
         if (pointsSteel[pointNumber].gameObject.activeSelf && steelNum != pointNumber)  //then the thief goes to him, if the object is already stolen, then select a new object
            {
             steelNum = pointNumber;
             myAgent.SetDestination(pointsSteel[steelNum].transform.position);
         }
         else
         {
             MovingThief();
         }
        if (Camera.main.GetComponent<CheckYourWork>().timeField.activeSelf)
            Camera.main.GetComponent<CheckYourWork>().TimeCheck(escapeZone.gameObject);

    }
    public void EscapeThief()
    {
        myAgent.SetDestination(escapeZone.gameObject.transform.position);
    }
    public void EndOfCheck()
    {
        OpenDoor[] allGlasses = FindObjectsOfType<OpenDoor>(); //Restoring all windows after the check is completed
        foreach (OpenDoor glass in allGlasses)
            if (glass.isWindowDoor)
                glass.ReturnGlass();

        steelNum = -1;
        pointNum = 0;
        foreach (GameObject point in pointsSteel)
        {
            point.SetActive(true);
        }
        pointsSteel.Clear();
        NavMeshModifierVolume[] allNavMeshModifierVolume = FindObjectsOfType<NavMeshModifierVolume>(); //Restoring all NavMeshModifierVolume and NavMeshModifier after the check is completed
        foreach (NavMeshModifierVolume navMeshModifierVolume in allNavMeshModifierVolume)
            navMeshModifierVolume.enabled = true;
        foreach (NavMeshModifier navMeshModifier in navMeshModifierNeedReturn)
            navMeshModifier.enabled = true;
        Camera.main.GetComponent<CameraSwitchPositionOnTop>().ButtonInterecteble();
        Destroy(gameObject);
    }
}
