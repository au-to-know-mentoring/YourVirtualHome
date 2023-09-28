using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DownloadBarProgress : MonoBehaviour
{
    public GameObject downloadCanvas;
    private DownloadHandler DownloadHandlerScript;

    public Slider ProgressSlider;
    public Text DownloadPercentText;

    string PercentString;

    int downloadBar;
    


    void Start()
	{
        DownloadHandlerScript = downloadCanvas.GetComponent<DownloadHandler>(); 

        ProgressSlider.maxValue = 100; 
    }
    
    // Update is called once per frame
    void Update()
    {
         if (DownloadHandlerScript.ProgressVar != null)
            {
            if (DownloadHandlerScript.ProgressVar.ProgressPercentage != 101)
            {
                string PercentString = Convert.ToString(DownloadHandlerScript.ProgressVar.ProgressPercentage); // converts for use in text component
                downloadBar = DownloadHandlerScript.ProgressVar.ProgressPercentage; // gets the int of ProgressPercantage 
                DownloadPercentText.text = PercentString + "%"; // writes percent
                ProgressSlider.value = downloadBar; // filles download bar 
            }
        }
       
    }
}
