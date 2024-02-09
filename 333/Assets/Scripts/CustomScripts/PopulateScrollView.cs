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

public class PopulateScrollView : MonoBehaviour
{
    public DownloadHandler downloadHandler;
	public GameObject parent;
	public GameObject buttonPrefab;
	private int count = 0;
	private bool StartCoutned;

	List<string> KeyNames = new List<string>();

	private void Start()
	{
		downloadHandler.ListModelFolders();
		// use player pref each line contains Name/ClientName and array int
		foreach (string key in downloadHandler.ListOfModelFolders)
		{
			AddModelButtonOnStart(PlayerPrefs.GetString("Model" + count), count);
			count++;
		}
	}

	public void SetModelPref(string Key,string Value)
	{
		
		PlayerPrefs.SetString(Key, Value);
	}

	/// <summary>
	/// Used to Add a new model button that has not been saved to player pref
	/// </summary>
	/// <param name="Key"></param>
	/// <param name="Name"></param>
	public void AddModelButton(string Key, string Name)
	{

			var buttonObject = Instantiate(buttonPrefab);
			var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
			//var ModelVal = buttonObject.GetComponent<modelValueInButton>();

			buttonText.text = Name;
			SetModelPref(Key, Name);
		
			buttonObject.transform.SetParent(parent.transform);
			buttonObject.transform.localScale = Vector3.one;
			buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
			modelValueInButton.modelVal = downloadHandler.ListOfModelFolders.Count - 1;
			modelValueInButton.downloadHandler = downloadHandler;


		
	}

	/// <summary>
	/// used to add a button that has previously been saved to playerpref
	/// </summary>
	/// <param name="ButtonText">Name/ClientName</param>
	/// <param name="myModelVal" >DownloadHandler.LoadModelToScene(myModelVal)</param>

	public void AddModelButtonOnStart(string ButtonText, int myModelVal)
	{

		var buttonObject = Instantiate(buttonPrefab);
		var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
		//var ModelVal = buttonObject.GetComponent<modelValueInButton>();


		buttonText.text = ButtonText;

		Debug.Log("Button Text: " + buttonText.text);

		buttonObject.transform.SetParent(parent.transform);
		buttonObject.transform.localScale = Vector3.one;
		buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

		buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

		modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
		modelValueInButton.modelVal = myModelVal;
		modelValueInButton.downloadHandler = downloadHandler;
	}
}
