using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.ComponentModel;
using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;

public class PopulateScrollView : MonoBehaviour
{
    public DownloadHandler downloadHandler;
	public GameObject parent;
	public GameObject buttonPrefab;
	private int Modelcount = 0;
	private int PrefCount = 1;
	private bool StartCoutned;

	List<string> KeyNames = new List<string>();

	private void Start()
	{


		downloadHandler.ListModelFolders();
		// use player pref each line contains Name/ClientName and array int
		foreach (string key in downloadHandler.ListOfModelFolders)
		{

			if (PlayerPrefs.GetString("FN" + "Model" + PrefCount) != "" && PlayerPrefs.GetString("LN" + "Model" + PrefCount) != "")
			{
				AddModelButtonOnStart(PlayerPrefs.GetString("FN" + "Model" + PrefCount), PlayerPrefs.GetString("LN" + "Model" + PrefCount), Modelcount);
				Modelcount++;
			}
		}
	}

	public void SetModelPref(string Key,string FnValue, string LnValue)
	{
		PlayerPrefs.SetString("FN" + Key, FnValue);
		PlayerPrefs.SetString("LN" + Key, LnValue);
	}

	/// <summary>
	/// Used to Add a new model button that has not been saved to player pref
	/// </summary>
	/// <param name="Key"></param>
	/// <param name="Name"></param>
	public void AddModelButton(string Key, string FirstName, string ClientName)
	{
			SetModelPref(Key, FirstName, ClientName);

			var buttonObject = Instantiate(buttonPrefab);
			modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
			modelValueInButton.FirstName.text = FirstName;
			modelValueInButton.ClientName.text = ClientName;
			//var ModelVal = buttonObject.GetComponent<modelValueInButton>();

			
		
			buttonObject.transform.SetParent(parent.transform);
			buttonObject.transform.localScale = Vector3.one;
			buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			
			modelValueInButton.modelVal = downloadHandler.ListOfModelFolders.Count - 1;
			modelValueInButton.downloadHandler = downloadHandler;


		
	}

	/// <summary>
	/// used to add a button that has previously been saved to playerpref
	/// </summary>
	/// <param name="FirstName">Name/ClientName</param>
	/// <param name="ClientName">Name/ClientName</param>
	/// <param name="myModelVal" >DownloadHandler.LoadModelToScene(myModelVal)</param>

	public void AddModelButtonOnStart(string FirstName, string ClientName, int myModelVal)
	{

		var buttonObject = Instantiate(buttonPrefab);


		modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
		modelValueInButton.FirstName.text = FirstName;
		modelValueInButton.ClientName.text = ClientName;
	

		buttonObject.transform.SetParent(parent.transform);
		buttonObject.transform.localScale = Vector3.one;
		buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

		buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

		modelValueInButton.modelVal = myModelVal;
		modelValueInButton.downloadHandler = downloadHandler;
	}
}
