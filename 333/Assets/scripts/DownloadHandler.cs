using System.Collections;
using System.IO;
using UnityEngine;
using System.Text;
using Dummiesman;
using UnityEngine.Networking;
using System.Net;
using System;
using System.IO.Compression;
using System.ComponentModel;
 using UnityEngine.UI;
 using TMPro;

public class DownloadHandler : MonoBehaviour
{
    // event for completion of download
    public event System.ComponentModel.AsyncCompletedEventHandler? DownloadFileCompleted;
    // path to zip
    string path = "";

    public InputField iField;
    string myInput;
    
    public void DownloadFromMyLink() {
        string myInput = iField.text;
        if (myInput != "") {
           DownloadFile(); 
           //DownloadFileCallback();
        }
    }


    void DownloadFile()
    {
        WebClient client = new WebClient();

        path = Application.persistentDataPath;
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        Uri uri = new Uri("localhost:3000/virtualhome-remote/getModel/");
        // call download function 
        client.DownloadFileAsync(uri, path);
    }

    void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
    {
        // extract to assets folder
        var loadedObject = new OBJLoader().Load(Application.persistentDataPath + "");
        // load object into world 
        loadedObject.gameObject.transform.Rotate(-90f, 0f, 0f, Space.World);
        // apply collision 
      

        // code for modelview scene
        // give reference of house to wand
      
        // spawns player near house
        //positionPlayer(loadedObject);
        
    }

    


  
   
}