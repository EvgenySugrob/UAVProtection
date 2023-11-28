using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
//Script should be attached on sensor zone in sensor(motion and vibro) prefab
public class SecureDetect : MonoBehaviour
{
    public string zoneName;
    public Renderer sensorZoneRender;
    public Material alarm;
    [SerializeField] Light sensZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<NavMeshAgent>() && zoneName == other.gameObject.GetComponent<Walk>().zoneName)
        {
            if (!Camera.main.GetComponent<CheckYourWork>().timeField.activeSelf) //turn on the timer on detection if it was not turned on
            {
                Camera.main.GetComponent<CheckYourWork>().TimerOn();
                
                NavMeshModifier[] allNavMeshModifier = FindObjectsOfType<NavMeshModifier>(); //search for all NavMeshModifier objects to disable them and then restore them 
                foreach (NavMeshModifier navMeshModifier in allNavMeshModifier)//if a thief is detected
                {
                    if (navMeshModifier.enabled)
                    {
                        other.gameObject.GetComponent<Walk>().navMeshModifierNeedReturn.Add(navMeshModifier);//if NavMeshModifaer is enabled we're save them in list for restore after check security
                    }
                    navMeshModifier.enabled = false;
                }
                NavMeshModifierVolume[] allNavMeshModifierVolume = FindObjectsOfType<NavMeshModifierVolume>();//NavMeshModifaerVolume it's a universally component which can  
                foreach (NavMeshModifierVolume navMeshModifierVolume in allNavMeshModifierVolume)//be disabled and restore after check without specifics
                    navMeshModifierVolume.enabled = false;

                Camera.main.GetComponent<CameraSwitchPositionOnTop>().walkableZone.GetComponent<NavMeshSurface>().BuildNavMesh();//after disabling all NavMesh area we needed rebild AI walking mesh (card)
            }

            sensZone.color = Color.red;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NavMeshAgent>())
            sensZone.color = Color.green;

    }
}
