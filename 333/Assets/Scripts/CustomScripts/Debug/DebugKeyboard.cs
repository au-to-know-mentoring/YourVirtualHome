using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeyboard : MonoBehaviour
{
   public DownloadHandler downloadHandler;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.O))
        {
          //  downloadHandler.DownloadFile();
        }  
    }
}
