using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for switching to the player controller/ Teleport controller
/// </summary>
public class PlayerSetup : MonoBehaviour
{

    public static PlayerSetup Instance;

    private void Awake() {
        if (Instance == null){
            Instance = this;
        }else {
            Destroy(this);
        }
    }
   
    [SerializeField] private GameObject SpawnHouse;
    [SerializeField] private GameObject SpawnHouseCube;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ControlPanelCanvas;

    [Tooltip("UI Info Helper Text")][SerializeField] private GameObject SettingsSceneRightControllerInfoHelper;
    [Tooltip("Teleport Info Helper Text")][SerializeField] private GameObject RightControllerInfoHelper;
    [Tooltip("Wand Info Helper Text")][SerializeField] private GameObject LeftControllerInfoHelper;

    /// <summary>
    /// Configuring the scene for the PlayerController
    /// </summary>
    public void SettingUpPlayerForModelScene(){

        Destroy(SpawnHouse);
        Destroy(SpawnHouseCube);

        MainMenuCanvas.active = false;
        ControlPanelCanvas.active = false;

        // Swap Button Layout Help Information text
        SettingsSceneRightControllerInfoHelper.active = false;
        RightControllerInfoHelper.active = true;
        LeftControllerInfoHelper.active = true;

        
        CustomTeleporter myTP = FindObjectOfType<CustomTeleporter>();
        myTP.enabled = true;

        Wand myWand = FindObjectOfType<Wand>();
        myWand.enabled = true;
    }

}
