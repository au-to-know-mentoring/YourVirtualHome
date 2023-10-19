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
using UnityEngine.SceneManagement;


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
    string myInput = ""; // for Code input 
    


    public void DownloadFromMyLink() {
       
         DownloadFile(); // only called from UI button press and not on application start
        
    }
    
    void DownloadFile()
    {
        string myInput = iField.text; // for Code Input
        
        //string myInput = iField.text;
        WebClient client = new WebClient();

        zipFile = Application.productName + ".zip";
        path = Application.persistentDataPath  + ".zip";
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        // get ProgressPercent for DownloadBarProgress.cs
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4); // + myInput

        Uri uri = new Uri("https://aumentoring.com.au/virtualhome-remote/getModel/935211"); //https://github.com/ATK-mentoring/fbx-examples/blob/main/ArchicadObjSize.zip localhost:3000/virtualhome-remote/getModel/380150 // needs to be fixed to use our code input needs to be online tho
        // call download function 
        Debug.Log(myInput);
        Debug.Log(uri);
        client.DownloadFileAsync(uri, path);
        
        
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

	public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
	{

		// extract to assets folder
		ZipFile.ExtractToDirectory(path, Application.persistentDataPath);

	} // download files don't import, different add the .obj .mtl (the model) stores a folder, what ever model in view port has a id to the files, click run , import the model to the modelviewScene
    public void SwitchToModelScene() // load modelview scene in background then import model to model view scene, then Switch from "Default Scene" to Model View Scene
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadYourAsyncScene());  
    }
    IEnumerator LoadYourAsyncScene() // load ModelViewScene in back ground
	{
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ModelScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //LoadModelToScene();
            yield return null;
        } 
      
       
    }
    public void LoadModelToScene() // used by ImportModelToModelViewScene.cs
	{


       
        Debug.Log(Application.persistentDataPath);
        var loadedObject = new OBJLoader().Load(Application.persistentDataPath +  "/TEST 8/" + "Vacation Home.obj", Application.persistentDataPath + "/TEST 8/" + "Vacation Home.mtl");
        
            // load object into world 
            loadedObject.gameObject.transform.Rotate(-90f, 0f, 0f, Space.World);
        Debug.Log("transform");
        // apply collision 
        WorldManager.ApplyCollidersToHouse(loadedObject);
        Debug.Log("collider");
        // code for modelview scene
        // give reference of house to wand
        FindObjectOfType<Wand>().setHouse(loadedObject);
        Debug.Log("wand");
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
    public void DeleteFilesWithButton() 
    {
        DeleteAllFiles();
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