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
public class DownloadHandler : MonoBehaviour
{

	// event for completion of download
	public event System.ComponentModel.AsyncCompletedEventHandler? DownloadFileCompleted;
	
	string path = ""; 
	string zipFile = ""; 
	string unZipFolderLocation; 
	string ICode; // saves 6-digit code to use when calling GetModelInfo in DownloadFileCallback();
	
	public List<string> ListOfModelFolders = new List<string>();
	public List<string> dirs = new List<string>();
	
	
	string[] OBJfiles;
	string[] ArrayMTLfiles;
	
	
	 public GameObject ModelHolderParent; // will be obsolete when settings scene has been introduced
	public string unkownPathTwo;
	

	public GetModelInfo getModelInfoScript;
	public Keyboard myKeyboardScript;
	

	void Start()
	{
		ListModelFolders();
	}

	public void DownloadFile(string Code)
	{
		ICode = Code;
		WebClient client = new WebClient();

		zipFile = Application.productName + ".zip";
		path = Application.persistentDataPath + "/" + Application.productName + ".zip";
		// links function  to event
		client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
		// get ProgressPercent for DownloadBarProgress.cs
		client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);

		Uri uri = new Uri("https://aumentoring.com.au/virtualhome-remote/getModel/" + Code);

		// call download function 
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
		unZipFolderLocation = Application.persistentDataPath + "/ " + Application.productName + "Model" + ListOfModelFolders.Count; // the extracted folder name

		ZipFile.ExtractToDirectory(path, unZipFolderLocation);
		ListModelFolders(); // upadtes the Model Folders List with new folder

		// Gets model Name/ClientName then instantiates a new button inside of our Model ScrollView with PopulateScrollView.cs
		GetModelInfo myGetModelInfo = FindObjectOfType<GetModelInfo>();
		myGetModelInfo.getModelInfo(ICode, "Model" + ListOfModelFolders.Count);
	}


	// May be removed, and instead turn ui controller off, and turn locomotion controller on

	//public void SwitchToModelScene() 
	//{
	//	Scene scene = SceneManager.GetActiveScene();
	//	StartCoroutine(LoadYourAsyncScene());
	//}
	//IEnumerator LoadYourAsyncScene() 
	//{
	//	UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ModelScene", LoadSceneMode.Single);

		
	//	while (!asyncLoad.isDone)
	//	{
	//		yield return null;
	//	}
	//}
	//->
	public void LoadModelToScene(int Choice) // used by ImportModelToModelViewScene.cs
	{

		string[] OBJfiles = Directory.GetFiles(ListOfModelFolders[Choice], "*.obj", SearchOption.AllDirectories);


		var objFilePath = OBJfiles[0];// get full path to the OBJ file


		string[] ArrayMTLfiles = Directory.GetFiles(ListOfModelFolders[Choice], "*.mtl", SearchOption.AllDirectories);

		var mtlFilePath = ArrayMTLfiles[0]; // get full path to the MTL file

		Debug.Log(mtlFilePath);
		Debug.Log(objFilePath);

		var loadedObject = new OBJLoader().Load(objFilePath, mtlFilePath); // imports the obj

		Vector3 OriginalScale = loadedObject.gameObject.transform.localScale;

		loadedObject.gameObject.transform.SetParent(ModelHolderParent.transform); // putting our model in a cube allowing for rotation
		loadedObject.gameObject.transform.localScale *= 0.025f;
		loadedObject.gameObject.transform.localPosition = Vector3.zero;

		DataManager.Instance.SetHouse(loadedObject);

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


	public void DeleteAllFiles()
	{

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