using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    [SerializeField] InputActionReference menuButton;
    [SerializeField] GameObject menuCanvas;

    [SerializeField] CustomTeleporter customTeleporter;
    [SerializeField] LineMeasurements lineMeasurements;

    [SerializeField] TMP_Text switchModeText;
    [Tooltip("OVR Camera Rig default, for adjusting player height")][SerializeField] GameObject playerCapsule;// for adjusting height



    // Start is called before the first frame update
    void Start()
    {
        menuButton.action.Enable();
        menuButton.action.performed += OpenMenu;

        switchModeText.text = "Switch To: " + "Tape Measure";
        
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (menuCanvas.activeSelf == true)
        {
            menuCanvas.SetActive(false);
        }
        else
        {
            menuCanvas.SetActive(true);
        }
    }

    public void SwitchMode()
    {
        if (customTeleporter.enabled == true)
        {

            switchModeText.text = "Switch To: " + "Teleport";

            lineMeasurements.enabled = true;
            lineMeasurements.MeasurementButton.action.Enable();

            lineMeasurements.lineRenderer.enabled = true;

            customTeleporter.enabled = false;
            // customTeleporter.lr.enabled = false;

            // customTeleporter.ti.SetActive(false);


            Debug.Log("LineMeasure: Is" + lineMeasurements.enabled + "Should be TRUE");
            Debug.Log("LineMeasure: Is" + customTeleporter.enabled + "Should be FALSE");
        }
        else
        {
            switchModeText.text = "Switch To: " + "Tape Measure";

            lineMeasurements.MeasurementButton.action.Disable();
            lineMeasurements.enabled = false;

            // lineMeasurements.lineRenderer.enabled = false;

            customTeleporter.enabled = true;
            // customTeleporter.lr.enabled = true;
            // customTeleporter.ti.SetActive(true);



            Debug.Log("LineMeasure: Is" + lineMeasurements.enabled + "Should be FALSE");
            Debug.Log("LineMeasure: Is" + customTeleporter.enabled + "Should be TRUE");
        }

        menuCanvas.SetActive(false);// close menu
    }

    public void BackToMenu()
    {
        FindObjectOfType<SettingsScenePlayer>().inModelScene = false;
        SceneManager.LoadScene(0);
    }

    public void Return()
    {
        menuCanvas.SetActive(false);
    }

    public void AdjustHeight(float AdjustValue)
    {
        playerCapsule.transform.position += new Vector3(0, AdjustValue, 0);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
