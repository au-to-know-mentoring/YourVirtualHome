using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for switching to the player controller/ Teleport controller
/// </summary>
public class PlayerSetup : MonoBehaviour
{

   
    [SerializeField] private GameObject SpawnHouse;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ControlPanelCanvas;

    [SerializeField] private GameObject SettingsSceneRightControllerInfoHelper;
    [SerializeField] private GameObject RightControllerInfoHelper;

    /// <summary>
    /// Configuring the scene for the PlayerController
    /// </summary>
    public void SettingUpPlayerForModelScene(){

        Destroy(SpawnHouse);
        MainMenuCanvas.active = false;
        ControlPanelCanvas.active = false;

        SettingsSceneRightControllerInfoHelper.active = false;
        RightControllerInfoHelper.active = true;

        
        CustomTeleporter myTP = FindObjectOfType<CustomTeleporter>();
        myTP.enabled = true;

        Wand myWand = FindObjectOfType<Wand>();
        myWand.enabled = true;
    }

}
