using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DownloadBarProgress : MonoBehaviour
{
   [SerializeField] private DownloadHandler DownloadHandlerScript;

    public Slider ProgressSlider;
    public TMP_Text DownloadPercentText;


    int downloadBar;
    


    void Start()
	{
       ProgressSlider.maxValue = 100; 
    }
 
    // Update is called once per frame
    void Update()
    {
        if (DownloadHandlerScript.ProgressVar != null)
        {
            if (DownloadHandlerScript.ProgressVar.ProgressPercentage <= 100) // 100 
            {
                string PercentString = Convert.ToString(DownloadHandlerScript.ProgressVar.ProgressPercentage); // converts for use in text component
                downloadBar = DownloadHandlerScript.ProgressVar.ProgressPercentage; // gets the int of ProgressPercantage 
                DownloadPercentText.text = PercentString + "%"; // writes percent
                ProgressSlider.value = downloadBar; // filles download bar 
            }
        }
    }
}
