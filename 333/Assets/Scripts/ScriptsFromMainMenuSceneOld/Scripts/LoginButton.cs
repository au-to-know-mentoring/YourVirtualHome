using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    public GameObject loginPanel;

    public GameObject mainMenuPanel;
    
    public Button btnClick;

    // Place Holder
    public InputField inputUser;

    // Place Holder
    public InputField inputPassword;

    public void Hide()
    {
        loginPanel.SetActive(false);
    }

    public void Show()
    {
        mainMenuPanel.SetActive(true);
    }

    private void Start()
    {
        btnClick.onClick.AddListener(GetInputOnClickHandler);
    }

    public void GetInputOnClickHandler()
    {
        Debug.Log("Login Output: " + "Password: " + inputPassword.text +" " + "Username: " + inputUser.text);
    }


}
