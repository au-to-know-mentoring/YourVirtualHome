using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public void ButtonCall()
    {
        Debug.Log("Button Test");
    }

    public void SceneButtonCall()
    {
        Debug.Log("Switching Scene");
    }


}
