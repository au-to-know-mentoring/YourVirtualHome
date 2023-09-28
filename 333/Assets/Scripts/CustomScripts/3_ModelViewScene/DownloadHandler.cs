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
// localhost:3000/virtualhome-remote/getModel/ 
public class DownloadHandler : MonoBehaviour
{
    // event for completion of download
    public event System.ComponentModel.AsyncCompletedEventHandler? DownloadFileCompleted;
    // path to zip
    string path = "";
    // zipFile name to delete on application quit
    string zipFile = "";
    public InputField iField; // forr Code input
    string myInput; // for Code input 
    


    
    public void DownloadFromMyLink() {
       
         DownloadFile(); // only called from UI button press and not on application start
        
    }
  
    void DownloadFile()
    {
        //string myInput = iField.text; // for Code Input
        
        //string myInput = iField.text;
        WebClient client = new WebClient();

        zipFile = Application.productName + ".zip";
        path = Application.persistentDataPath  + ".zip";
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        // get ProgressPercent for DownloadBarProgress.cs
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4); 

        Uri uri = new Uri("https://github.com/ATK-mentoring/fbx-examples/raw/main/ArchicadObjSize.zip"); //https://github.com/ATK-mentoring/fbx-examples/blob/main/ArchicadObjSize.zip localhost:3000/virtualhome-remote/getModel/380150 // needs to be fixed to use our code input needs to be online tho
        // call download function 
        
        client.DownloadFileAsync(uri, path);
        Debug.Log(uri);
    }
    public DownloadProgressChangedEventArgs ProgressVar; // for DownloadBarProgress.cs to use Percentage value
    public void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
    {
        ProgressVar = e;
        // Displays the operation identifier, and the transfer progress.
        Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
            (string)e.UserState,
            e.BytesReceived,
            e.TotalBytesToReceive,
            e.ProgressPercentage);// for DownloadBarProgress.cs to get percentage
    }

	void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
	{

		// extract to assets folder
		ZipFile.ExtractToDirectory(path, Application.persistentDataPath);

		var loadedObject = new OBJLoader().Load(Application.persistentDataPath + "/TEST 8/" + "Vacation Home.obj", Application.persistentDataPath + "/TEST 8/" + "Vacation Home.mtl");

		// load object into world 
		loadedObject.gameObject.transform.Rotate(-90f, 0f, 0f, Space.World);
		// apply collision 
		WorldManager.ApplyCollidersToHouse(loadedObject);

		// code for modelview scene
		// give reference of house to wand
		FindObjectOfType<Wand>().setHouse(loadedObject);
		// spawns player near house
		//positionPlayer(loadedObject);

	}

	void positionPlayer(GameObject house)
    {
        GameObject player = FindObjectOfType<Wand>().gameObject;
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != house)
            {
                player.transform.position = house.transform.position;
            }
        }
    }


    private void OnApplicationQuit()
    {
        DeleteAllFiles();
        Debug.Log(zipFile);
        Debug.Log(path);
    }

    public void DeleteAllFiles() {
        ClearFiles("/" + "TEST 8"); // will move Delete Model function to the model previews panel
        ClearFiles(".zip"); // deletes the zip 
    }
   
    public void ClearFiles(string path)
    {
        string target = Application.persistentDataPath + path;
        if (path.Substring(path.Length - 4, 1) == ".")
        {
            File.Delete(target);
        }
        else
        {
            Directory.Delete(target, true);
        }

    }
}