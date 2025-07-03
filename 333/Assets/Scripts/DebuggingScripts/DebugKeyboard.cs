using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugKeyboard : MonoBehaviour
{
    public SettingsScenePlayer SettingsScenePlayer;
    public TMP_InputField InputField;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.O))
        {
            //SettingsScenePlayer.applyDownloadCode(InputField);

		}  
    }
}
