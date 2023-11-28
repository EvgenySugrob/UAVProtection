using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
/*
 * This script should attached on DoorSens objoct on scene 
 * but then ve attach them only one object in connect must have this script
 * otherwise alarm will work out twice (non-correctable)
*/
public class AlarmOnOpenDoor : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<SetOpenDoorSensor>() && gameObject.GetComponent<SetOpenDoorSensor>().isSetSensor 
            && Camera.main.transform.parent.gameObject.GetComponent<CameraControlerOnTop>())
        {
            if (!Camera.main.GetComponent<CheckYourWork>().timeField.activeSelf) //turn on the timer on detection if it was not turned on
            {
                Camera.main.GetComponent<CheckYourWork>().TimerOn();

                NavMeshModifier[] allNavMeshModifier = FindObjectsOfType<NavMeshModifier>(); //search for all NavMeshModifier objects to disable them and then restore them 
                foreach (NavMeshModifier navMeshModifier in allNavMeshModifier)//if a thief is detected
                {
                    if (navMeshModifier.enabled)
                    {
                        GameObject thief = FindObjectOfType<NavMeshAgent>().gameObject; //if NavMeshModifaer is enabled we're save them in list for restore after check security
                        thief.GetComponent<Walk>().navMeshModifierNeedReturn.Add(navMeshModifier);
                    }
                    navMeshModifier.enabled = false;
                }
                NavMeshModifierVolume[] allNavMeshModifierVolume = FindObjectsOfType<NavMeshModifierVolume>(); //NavMeshModifaerVolume it's a universally component which can  
                foreach (NavMeshModifierVolume navMeshModifierVolume in allNavMeshModifierVolume) //be disabled and restore after check without specifics
                    navMeshModifierVolume.enabled = false;

                Camera.main.GetComponent<CameraSwitchPositionOnTop>().walkableZone.GetComponent<NavMeshSurface>().BuildNavMesh(); //after disabling all NavMesh area we needed rebild AI walking mesh (card)
            }
        }
    }
}
