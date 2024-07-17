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

/// <summary>
/// Handles downloading, Unzipping and deletion of the model
/// </summary>
public class DownloadHandler : MonoBehaviour
{

	// event for completion of download
	public event System.ComponentModel.AsyncCompletedEventHandler? DownloadFileCompleted;
	
	string path = ""; 
	string zipFile = ""; 
	string unZipFolderLocation; 
	string ICode; // saves 6-digit code to use when calling GetModelInfo in DownloadFileCallback();
	
	public List<string> ListOfModelFolders = new List<string>();
	
	
	public GameObject ModelHolderParent; // will be obsolete when settings scene has been introduced
	public GameObject loadingCanvas;
	public Canvas canvas;
	
	void Start()
	{	
		
		// DownloadFile("577485");
		//get loading canvas object
		loadingCanvas = GameObject.Find("LoadingCanvas");
		// loadingCanvas.SetActive(false);
		canvas = loadingCanvas.GetComponent<Canvas>();
		canvas.enabled = false;

		
		ListModelFolders();
		

	}


	/// <summary>
	/// Calls the virtualhome backend and download a model via the model code
	/// </summary>
	/// <param name="Code">Model Code</param>
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

		if (e.ProgressPercentage == 0) {
			// Animates download bar, and it's percentage text
			StartCoroutine(FindObjectOfType<AddModelDownloadStarted>().DownloadSliderProgress());
		}
	}

	/// <summary>
	/// Makes a list of model folders, Used for listing models on the scroll view and importing models
	/// </summary>
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
			Debug.Log(dir);

			ListOfModelFolders.Add(dir);
		}
	}

	/// <summary>
	/// Unzips model to PersistentDataPath
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
	{
		unZipFolderLocation = Application.persistentDataPath + "/" + Application.productName + "Model" + ListOfModelFolders.Count; // the extracted folder name

		ZipFile.ExtractToDirectory(path, unZipFolderLocation);

		// Get model information, Create .txt file and write model info
		StartCoroutine(GetModelInfo.Instance.SaveJsonRequest(ICode, Application.persistentDataPath + "/" + Application.productName + "Model" + ListOfModelFolders.Count + "/" + "jsonEncode.txt"));


		ListModelFolders(); // upadtes the Model Folders List with new folder
	}

	/// <summary>
	/// Loads model to the Dollhouse
	/// </summary>
	/// <param name="ModelId">Model Id</param>
	public void LoadModelToScene(int ModelId) // used by ImportModelToModelViewScene.cs
	{

		// gets all .obj files in directory
		string[] OBJfiles = Directory.GetFiles(ListOfModelFolders[ModelId], "*.obj", SearchOption.AllDirectories);


		var objFilePath = OBJfiles[0];// get full path to the OBJ file

		// gets all .mtl files in directory
		string[] ArrayMTLfiles = Directory.GetFiles(ListOfModelFolders[ModelId], "*.mtl", SearchOption.AllDirectories);

		var mtlFilePath = ArrayMTLfiles[0]; // get full path to the MTL file



	    #region Model Information
		// Loads model to Scene
		var loadedObject = new OBJLoader().Load(objFilePath, mtlFilePath); // imports the obj

		Vector3 OriginalScale = loadedObject.gameObject.transform.localScale;

		// nest model inside of dollhouse
		loadedObject.gameObject.transform.SetParent(ModelHolderParent.transform); // putting our model in a cube allowing for rotation

		// Scales down so it can be configured with the control panel
		loadedObject.gameObject.transform.localScale *= 0.025f;
		loadedObject.gameObject.transform.localPosition = Vector3.zero;
		
		// SetHouse Variable
		DataManager.Instance.SetHouse(loadedObject);

		Debug.Log("transform");
		// apply collision 
		WorldManager.Instance.ApplyCollidersToHouse(loadedObject);
		
		Debug.Log("collider");

		#endregion
		// code for modelview scene
		// give reference of house to wand
		FindObjectOfType<Wand>().setHouse(loadedObject);
		Debug.Log("wand");
		// spawns player near house
		// positionPlayer(loadedObject);
		canvas.enabled = false;
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

	public void DeleteModel(string DeletionModelPath)
	{
		Directory.Delete(DeletionModelPath, true);
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