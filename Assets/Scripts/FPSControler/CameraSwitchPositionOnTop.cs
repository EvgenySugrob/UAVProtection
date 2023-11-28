using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Unity.AI.Navigation;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Michsky.MUIP;

public class CameraSwitchPositionOnTop : MonoBehaviour
{
    public Transform cameraTopHolder, cameraHolder;
    public Transform playerCamera;

    Quaternion targetAngle = Quaternion.Euler(45f, 0.0f, 0.0f);
    Vector3 targetPosition = new Vector3(0.0f, 15f, -15f);

    public bool isMove;
    public bool cameraMove;
    public bool accessReplace;

    public float speedRotation = 0.5f;
    public float speed = 1f;

    public CameraControlerOnTop cameraControlerOnTop;

    public GameObject roof;
    public GameObject storageRoof;

    [SerializeField] public FirstPersonController fpsControler;
    public GameObject walkableZone;
    [SerializeField] List<Transform> steelObject;

    public List<ObjectsToBurn> objectsToBurn;

    [SerializeField] TMP_Dropdown dropdownScenario;

    public GameObject inventoryPanel;
    public ShowInventory showInventory;

    public GameObject stopBt, replaceBt, checkBt;

    [SerializeField] List<GameObject> burningZoneTr;

    public ShowerOnOff showerOnOff;

    public GameObject progressBarGroup;

    public ReplaceButtonActivateDiactivate uiWithReplace;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var findObjectsToBurn = FindObjectsOfType<ObjectsToBurn>();
        for (int i = 0; i < findObjectsToBurn.Length; i++)
        {
            objectsToBurn.Add(findObjectsToBurn[i]);
        }

        var allsteelObjects = FindObjectsOfType<SteelScript>();
        foreach (var steel in allsteelObjects)
        {
            steelObject.Add(steel.gameObject.transform);
        }

        var burningZone = FindObjectsOfType<CombustionZone>();
        for (int i = 0; i < burningZone.Length; i++)
        {
            burningZoneTr.Add(burningZone[i].gameObject);
        }
        SwitchCameraOnTop();
    }

    private void Update()
    {
        //Checking the result after placing the sensors. Move the camera to the top position.

        if (isMove)
        {
            if (!cameraControlerOnTop.enabled)
            {
                playerCamera.rotation = Quaternion.Lerp(playerCamera.transform.rotation, targetAngle, speedRotation * Time.deltaTime);
                playerCamera.position = Vector3.MoveTowards(playerCamera.position, targetPosition, speed * Time.deltaTime);
                stopBt.SetActive(false);
                checkBt.SetActive(false);
            }

            if ((Vector3.Distance(playerCamera.transform.position, targetPosition) < 0.5f || cameraControlerOnTop.enabled) && cameraMove)
            {
                cameraMove = false;
                isMove = false;
                cameraControlerOnTop.enabled = true;
                uiWithReplace.replaceButtonOff.GetComponent<ButtonManager>().enabled = true;
                //uiWithReplace.replaceButtonOff.GetComponent<Button>().interactable = true;  // Интерактивность новых кнопок

                if (accessReplace)
                {
                    stopBt.SetActive(true);
                    checkBt.SetActive(false);
                    // Selecting a test scenario. Validates values ??via ScenarioDropdown
                    switch (dropdownScenario.value)
                    {
                        case 0:
                            {
                                Camera.main.GetComponent<CheckYourWork>().statsField.SetActive(false);
                                progressBarGroup.SetActive(true);
                                RandomActivationParticleSystem();
                                LightControl(true,true);
                                LightControl(false, false);
                                break;
                            }
                        case 1:
                            {
                                Camera.main.GetComponent<CheckYourWork>().statsField.SetActive(true);
                                progressBarGroup.SetActive(false);
                                walkableZone.GetComponent<NavMeshSurface>().BuildNavMesh();
                                gameObject.GetComponent<CheckYourWork>().SpawnThief(steelObject);
                                LightControl(false, true);
                                LightControl(true, false);
                                break;
                            }
                    }
                }
                
            }
        }
    }

    //Called when StopBt is clicked
    public void StopCheck()
    {
        FireDiactivation();
        SmokeDioxidDiactivation();
        if (gameObject.GetComponent<CheckYourWork>().realThief != null)
            gameObject.GetComponent<CheckYourWork>().RemoveThief();

        ButtonInterecteble();
    }


    public void SwitchCameraOnTop()
    {
        StopAllCoroutines();

        inventoryPanel.SetActive(false);
        showInventory.enabled = false;
        fpsControler.enabled = false; 
        roof.SetActive(false);
        storageRoof.SetActive(false);
        if (playerCamera.transform.parent.GetComponent<Builds>()) 
            playerCamera.transform.parent.GetComponent<Builds>().crosshair.SetActive(false);
        playerCamera.transform.SetParent(cameraTopHolder);

        isMove = true;
        cameraMove = true;

    }


    //Random launch of one of the ignition sources
    public void RandomActivationParticleSystem()
    {
        foreach (GameObject zone in burningZoneTr)
        {
            zone.GetComponent<ZoneBurnPercentage>().stopPercent = false;
            zone.GetComponent<ZoneBurnPercentage>().textProcent.text = "0%";
            zone.GetComponent<ZoneBurnPercentage>().fillProgress.fillAmount = 0f;
        }
        var randomActivarion = objectsToBurn[Random.Range(0, objectsToBurn.Count)];
        randomActivarion.transform.GetChild(0).GetComponent<SmokeDioxidActivationDiactivation>().SmokeDioxidActivation();
        randomActivarion.transform.GetChild(1).GetComponent<FireActivationDiactivation>().FireActivation();
    }

    [System.Obsolete]
    public void FireDiactivation()
    {
        var fireObject = FindObjectsOfType<FireActivationDiactivation>();
        for (int i = 0; i < fireObject.Length; i++)
        {
            fireObject[i].FireDiactivation();
        }
    }

    [System.Obsolete]
    public void SmokeDioxidDiactivation()
    {
        var smokeDioxidObject = FindObjectsOfType<SmokeDioxidActivationDiactivation>();
        for (int i = 0; i < smokeDioxidObject.Length; i++)
        {
            smokeDioxidObject[i].SmokeDioxidDiactivation();
        }
    }


    public void StopButtonAndReplaceObjectButton()
    {
        if (!stopBt.activeSelf)
        {
            stopBt.SetActive(true);
        }
    }

    public void ReplaceOn()
    {
        foreach (GameObject zone in burningZoneTr)
        {
            if (zone.GetComponent<ZoneBurnPercentage>().zoneNamePercent != "Zone7")
            {
                zone.GetComponent<BoxCollider>().center = new Vector3(zone.GetComponent<BoxCollider>().center.x, -3.3f, zone.GetComponent<BoxCollider>().center.z);
                foreach (GameObject dragObject in zone.GetComponent<CombustionZone>().dragObject)
                {
                    dragObject.GetComponent<MouseDrag>().onEnter = false;
                    dragObject.GetComponent<MouseDrag>().dragIsOn = true;
                    dragObject.GetComponent<Outline>().OutlineColor = new Color(1, 0.3764706f, 0, 1);
                }
            }
        }
    }
    //Applying the change and navigating to sensor placement
    public void ReplaceOff()
    {
        foreach (GameObject zone in burningZoneTr)
        {
            if (zone.GetComponent<ZoneBurnPercentage>().zoneNamePercent != "Zone7")
            {
                zone.GetComponent<BoxCollider>().center = new Vector3(zone.GetComponent<BoxCollider>().center.x, -0.027354f, zone.GetComponent<BoxCollider>().center.z);
                foreach (GameObject dragObject in zone.GetComponent<CombustionZone>().dragObject)
                {
                    dragObject.GetComponent<MouseDrag>().dragIsOn = false;
                }
            }            
        }
        accessReplace=true;
        TeleportCameraToCameraHolder();

    }

    public void ShowerDeactivation()
    {
        FireDiactivation();
        SmokeDioxidDiactivation();
        StartCoroutine(ShowerDeactivation(5f));
    }

    IEnumerator ShowerDeactivation(float time)
    {
        yield return new WaitForSeconds(time);
        showerOnOff.ShowerOff();
        ButtonInterecteble();

    }

    public void ButtonInterecteble()
    {
        stopBt.SetActive(false);
        checkBt.SetActive(true);
    }

    public void StopProgressBar()
    {
        foreach (GameObject zone in burningZoneTr)
        {
            zone.GetComponent<ZoneBurnPercentage>().stopPercent = true;
        }
    }

    private void TeleportCameraToCameraHolder()
    {
        cameraControlerOnTop.SetPosition();
        cameraTopHolder.position = new Vector3(0f,0f, 0f);
        cameraTopHolder.rotation = Quaternion.Euler(0f, 0.0f, 0.0f);
        playerCamera.transform.SetParent(cameraHolder);
        cameraControlerOnTop.enabled = false;
        playerCamera.rotation = Quaternion.Euler(0f, 0.0f, 0.0f);
        playerCamera.localPosition = new Vector3(0f, 0.498f, 0f);

        inventoryPanel.SetActive(true);
        showInventory.enabled = true;
        fpsControler.enabled = true;
        roof.SetActive(true);
        storageRoof.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Display of light sensors depending on the selected scenario
    private void LightControl(bool isSwitch, bool isOn)
    {
        Light[] lightSensors = FindObjectsOfType<Light>();

        foreach (Light lightSensor in lightSensors)
        {
            if (lightSensor.transform.parent)
            {
                if ((lightSensor.transform.parent.GetComponent<SensorDetect>() || lightSensor.transform.parent.GetComponent<SmokeSensorDetect>()) && isSwitch)
                {
                    lightSensor.enabled = isOn;
                }
                else if ((lightSensor.transform.parent.GetComponent<SecureDetect>() || lightSensor.transform.parent.GetComponent<SecureGlass>()) && !isSwitch)
                {
                    lightSensor.enabled = isOn;
                }
            }

        }
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
