using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackToSettingsScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputActionReference ReturnButton;

    private void Awake() 
    {
        ReturnButton.action.Enable();
        ReturnButton.action.performed += Return;
    }

   void Return(InputAction.CallbackContext context)
   {
        SceneManager.LoadScene(0);
   }
}
