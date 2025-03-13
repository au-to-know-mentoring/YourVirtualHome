using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BackToSettingsScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputActionReference ReturnButton;
    [SerializeField] InputActionReference CancelButton;
    [SerializeField] GameObject Panel;

    [SerializeField] Wand wand;
    [SerializeField] CustomTeleporter customTeleporter;

    private void Awake() 
    {
        CancelButton.action.Enable();
        CancelButton.action.performed += Cancel;


        ReturnButton.action.Enable();
        ReturnButton.action.performed += Return;
    }

    
    void Cancel(InputAction.CallbackContext context){
        Panel.SetActive(false);
        customTeleporter.enabled = true;
        wand.enabled = true;
    }
   void Return(InputAction.CallbackContext context)
   {
        if (FindObjectOfType<SettingsScenePlayer>().inModelScene != false)
            {
                customTeleporter.enabled = false;
                wand.enabled = false;
                

                if (Panel.activeSelf == true){
                    FindObjectOfType<SettingsScenePlayer>().inModelScene = false;
                    SceneManager.LoadScene(0);

                }else {
                    Panel.SetActive(true);
                }
            }
    }
    
}
