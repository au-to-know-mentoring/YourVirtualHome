using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDownloadHandler : MonoBehaviour
{
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<DownloadHandler>().DownloadFile("564052");
        }
    }
}
