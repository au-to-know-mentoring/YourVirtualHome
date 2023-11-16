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
using System.Collections.Generic;

//using static System.Net.Mime.MediaTypeNames;


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
    public string myInput = ""; // for Code input 
   
    public List<string> ListOfModelFolders = new List<string>();

    private string SaveDownloadModelString = "ModelsDownloaded";

    public int ModelCountSave;
    public string unZipFolderLocation;
   
    private string[] dirs;
    string[] OBJfiles;
    string[] ArrayMTLfiles;


    public string unkownPathTwo;

    public int choiceTest;

    public bool PRESSME;

    public int showSelectInt;
    public int modelSelectInt { get; set; }
    string modelSelectKey = "ModelNum";

    private bool modelHasLoaded;

    public GetModelInfo getModelInfoScript;

    public Keyboard myKeyboardScript;
	// change the choiceTest INT to change the folder you are using in ListOfModelFolders


	public void Awake()
	{
        modelSelectInt = PlayerPrefs.GetInt(modelSelectKey);
        Debug.Log(modelSelectInt + gameObject.name);
	}
    public void setModelSelect()
	{
        PlayerPrefs.SetInt(modelSelectKey, choiceTest);
	}
	public TMP_Dropdown modelSelectDropDown;
    
    public void Update()
	{
        Debug.Log(Application.persistentDataPath + "/ " + Application.productName + "Model" + ListOfModelFolders.Count);
        myInput = myKeyboardScript.iField.text;
        Debug.Log(myInput);
        showSelectInt = modelSelectInt;
        setModelSelect();
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "ModelScene")
		{
            if (modelHasLoaded == false)
			{
                modelHasLoaded = true;
                LoadModelToScene();
            }
		}
    }

	public void Start()
	{
  
        ListModelFolders();

        unZipFolderLocation = Application.persistentDataPath + "Model";
 
    }
   
	public void DownloadFromMyLink() {
       
         DownloadFile(); // only called from UI button press and not on application start
    }
    
    public void DownloadFile()
    {
       // myInput = iField.text; // for Code Input
        
        //string myInput = iField.text;
        WebClient client = new WebClient();

        zipFile = Application.productName + ".zip";
        path = Application.persistentDataPath  + "/"+ Application.productName+".zip";
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        // get ProgressPercent for DownloadBarProgress.cs
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4); // + myInput

        Uri uri = new Uri("http://aumentoring.com.au/virtualhome-remote/getModel/460893"); //https://github.com/ATK-mentoring/fbx-examples/blob/main/ArchicadObjSize.zip localhost:3000/virtualhome-remote/getModel/380150 // needs to be fixed to use our code input needs to be online tho
        // call download function 
        Debug.Log(myInput);
        Debug.Log(uri);
        client.DownloadFileAsync(uri, path);

		//getModelInfoScript.getModelInfo();
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
    public void ListModelFolders() 
	{
        var headFolderPath = Path.GetDirectoryName(Application.persistentDataPath); // get's parent Folder
        
        dirs = new string[250];
        dirs = Directory.GetDirectories(headFolderPath, Application.productName + "Model*", SearchOption.TopDirectoryOnly);  

        ListOfModelFolders.Clear();

        foreach (string dir in dirs)
        {
            
            Debug.Log(dir); 
            ListOfModelFolders.Add(dir);
            
            Debug.Log(dir);
        }
        

    }

	public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
	{
            
             
             unZipFolderLocation = Application.persistentDataPath + "/ " +Application.productName +"Model" + ListOfModelFolders.Count; // the extracted folder name
             
             ZipFile.ExtractToDirectory(path, unZipFolderLocation);
             ListModelFolders(); // upadtes the Model Folders List with new folder


        // extract to download location


    } 
  

    // download files don't import, different add the .obj .mtl (the model) stores a folder, what ever model in view port has a id to the files, click run , import the model to the modelviewScene
    public void SwitchToModelScene() // load model view scene in background then import model to model view scene, then Switch from "Default Scene" to Model View Scene
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadYourAsyncScene());
        
    }
    IEnumerator LoadYourAsyncScene() // load ModelViewScene in back ground
	{
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ModelScene", LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //LoadModelToScene();
            yield return null;
            
        } 
        
      
       
    }
  
    public void LoadModelToScene() // used by ImportModelToModelViewScene.cs
	{
      
       foreach (string dir in ListOfModelFolders)
		{
           unkownPathTwo = dir;
        }
        
        Debug.Log(unkownPathTwo);
       
        string[] OBJfiles = Directory.GetFiles(unkownPathTwo,"*.obj", SearchOption.AllDirectories);
        
        
        var objFilePath = OBJfiles[0];// get full path to the OBJ file

        string[] ArrayMTLfiles = Directory.GetFiles(unkownPathTwo, "*.mtl", SearchOption.AllDirectories);
       
        var mtlFilePath = ArrayMTLfiles[0]; // get full path to the MTL file

        Debug.Log(mtlFilePath);

        
        var loadedObject = new OBJLoader().Load(objFilePath, mtlFilePath); // imports the obj
        
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


    public void OnApplicationQuit()
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
       // ClearFiles("/" + "TEST 8"); // will move Delete Model function to the model previews panel
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