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
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

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
    public InputField folderSelectIField; 
    public string myInput = ""; // for Code input 
   
    public List<string> ListOfModelFolders = new List<string>();
    public List<string> dirs = new List<string>();

    public string unZipFolderLocation;
    //<- Import OBJ files
    string[] OBJfiles;
    string[] ArrayMTLfiles;
	//->

    //<- 
	[HideInInspector] public GameObject ModelHolderParent; // will be obsolete when settings scene has been introduced
	public string unkownPathTwo; 
    //->

	//<- save selected model value
	public int choiceTest;
    public int showSelectInt;
	public int choose;
	public int modelSelectInt { get; set; }
    string modelSelectKey = "ModelNum";
    //->
    //<- Scripts References
    public GetModelInfo getModelInfoScript;
    public Keyboard myKeyboardScript;
    //->

	public Text FolderCount; // for debugging on the ui
	public TMP_Dropdown modelSelectDropDown;

    
	

	public void setModelSelect()
	{
        PlayerPrefs.SetInt(modelSelectKey, choiceTest);
	}
	
	public void Start()
	{
		modelSelectInt = PlayerPrefs.GetInt(modelSelectKey);
		Debug.Log(modelSelectInt + gameObject.name);


		ListModelFolders();
	}
	public void Update()
	{
        
        if (FolderCount != null)
        {
			FolderCount.text = ListOfModelFolders.Count.ToString();
		}

	   // myInput = myKeyboardScript.iField.text;
       // Debug.Log(myInput);

        
    }

	
   
	public void DownloadFromMyLink() {
       
         DownloadFile(); // only called from UI button press and not on application start
    }

    public void DownloadFile()
    {

		myInput = myKeyboardScript.iField.text;
		WebClient client = new WebClient();

        zipFile = Application.productName + ".zip";
        path = Application.persistentDataPath + "/" + Application.productName + ".zip";
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        // get ProgressPercent for DownloadBarProgress.cs
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4); 

       
        Uri uri = new Uri("http://aumentoring.com.au/virtualhome-remote/getModel/" + myInput); 
        // call download function 
        Debug.Log(myInput);
        Debug.Log(uri);
      
        client.DownloadFileAsync(uri, path);

		CheckIfCodeIsValid();
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
    public void CheckIfCodeIsValid()
    {
		if (ProgressVar.ProgressPercentage > 0)
		{
			Debug.Log("true");
		}
		else { Debug.Log("False");
        }
	}
    public void ListModelFolders() 
	{
		unZipFolderLocation = Application.persistentDataPath + "/" + Application.productName + "Model";
		var headFolderPath = Path.GetDirectoryName(unZipFolderLocation);
		headFolderPath += "/";

		string pattern = Application.productName + "Model\\d+$"; // getting the number at the end of the directory

		string[] dirsl = Directory.GetDirectories(headFolderPath, "*", SearchOption.AllDirectories)
			.Where(dir => Regex.IsMatch(dir, pattern))
			.ToArray();
        ListOfModelFolders.Clear();

		foreach (string dir in dirsl)
		{
			ListOfModelFolders.Add(dir);
		}
	}

	public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
	{

             unZipFolderLocation = Application.persistentDataPath + "/ " +Application.productName +"Model" + ListOfModelFolders.Count; // the extracted folder name
             
             ZipFile.ExtractToDirectory(path, unZipFolderLocation);
             ListModelFolders(); // upadtes the Model Folders List with new folder
             
                

        // extract to download location


    } 
  

    //<-    loads to ModelViewScene // will be obsolete once Settings scene is introduced
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
			setModelSelect(); 
			//LoadModelToScene();
			yield return null;
		}
    }
    //->
    public void LoadModelToScene() // used by ImportModelToModelViewScene.cs
	{
      
      foreach (string dir in ListOfModelFolders)
		{
         unkownPathTwo = dir;
        }
        
        Debug.Log("UNKOWN PATH: " + unkownPathTwo);


		string[] OBJfiles = Directory.GetFiles(ListOfModelFolders[choose], "*.obj", SearchOption.AllDirectories);
        
        
        var objFilePath = OBJfiles[0];// get full path to the OBJ file


        string[] ArrayMTLfiles = Directory.GetFiles(ListOfModelFolders[choose], "*.mtl", SearchOption.AllDirectories);
       
        var mtlFilePath = ArrayMTLfiles[0]; // get full path to the MTL file

        Debug.Log(mtlFilePath);
		Debug.Log(objFilePath);

		var loadedObject = new OBJLoader().Load(objFilePath, mtlFilePath); // imports the obj


        loadedObject.gameObject.transform.SetParent(ModelHolderParent.transform); // putting our model in a cube allowing for rotation
        loadedObject.gameObject.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        loadedObject.gameObject.transform.localPosition = Vector3.zero;
		
        
        
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