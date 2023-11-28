using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class SecureGlass : MonoBehaviour
{
    public string zoneName;
    public Renderer sensorZoneRender;
    public Material alarm;
    public List<GameObject> glasses;
    void Update()
    {
        if (glasses.Count != 0)
            foreach (GameObject glass in glasses)
            {
                if (!glass.activeSelf)
                {
                    if (!Camera.main.GetComponent<CheckYourWork>().timeField.activeSelf)
                    {
                        Camera.main.GetComponent<CheckYourWork>().TimerOn(); //turn on the timer on detection if it was not turned on
                        NavMeshModifier[] allNavMeshModifier = FindObjectsOfType<NavMeshModifier>();//search for all NavMeshModifier objects to disable them and then restore them
                        foreach (NavMeshModifier navMeshModifier in allNavMeshModifier)//if a thief is detected
                        {
                            if (navMeshModifier.enabled)
                            {
                                GameObject thief = FindObjectOfType<NavMeshAgent>().gameObject;
                                thief.GetComponent<Walk>().navMeshModifierNeedReturn.Add(navMeshModifier);//if NavMeshModifaer is enabled we're save them in list for restore after check security
                            }
                            navMeshModifier.enabled = false;
                        }
                        NavMeshModifierVolume[] allNavMeshModifierVolume = FindObjectsOfType<NavMeshModifierVolume>();//NavMeshModifaerVolume it's a universally component which can 
                        foreach (NavMeshModifierVolume navMeshModifierVolume in allNavMeshModifierVolume)//be disabled and restore after check without specifics
                            navMeshModifierVolume.enabled = false;

                        Camera.main.GetComponent<CameraSwitchPositionOnTop>().walkableZone.GetComponent<NavMeshSurface>().BuildNavMesh(); //after disabling all NavMesh area we needed rebild AI walking mesh (card)
                    }
                }
            }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<GlassIdent>() && !glasses.Contains(other.gameObject))
            if (other.gameObject.GetComponent<GlassIdent>().zoneName == zoneName)
            {
                glasses.Add(other.gameObject); 
                other.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<OpenDoor>().needBrokeGlass = false; //if sensor on breaking glass is set thief takes command that he must open window
            }
    }
}
