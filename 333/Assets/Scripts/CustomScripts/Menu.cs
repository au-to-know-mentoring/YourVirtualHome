using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    [SerializeField] InputActionReference menuButton;
    [SerializeField] GameObject menuCanvas;



    // Start is called before the first frame update
    void Start()
    {
        menuButton.action.Enable();
        menuButton.action.performed += OpenMenu;
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        Debug.Log("OpenMenu Pressed:");
        if (menuCanvas.activeSelf == true)
        {
            Debug.Log("OpenMenu Closed:");

            menuCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("OpenMenu Opened:");

            menuCanvas.SetActive(true);
        }
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

    // public void AdjustHeight()
    // {

    // }

    public void QuitApp()
    {
        Application.Quit();
    }
}
