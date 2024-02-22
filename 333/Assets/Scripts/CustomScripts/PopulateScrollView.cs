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
using UnityEngine.Windows;

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
		// use player pref each line contains Name/ClientName and array int
		//foreach(string  key in KeyNames)
		//{
		//	AddModelButton(key);
		//}

		
	}

	public void SetModelPref(string Value)
	{
		KeyNames.Add(Value);
		PlayerPrefs.SetString(KeyNames[KeyNames.Count], Value);
	}

	private void Update()
	{
		if(UnityEngine.Input.GetKeyDown(KeyCode.L))
		{
			AddModelButton("poopoo");
		}
	}

	public void AddModelButton(string Name)
	{
		//if (count < downloadHandler.ListOfModelFolders.Count)
		//{
		//	count++;

		//	if (count > downloadHandler.ListOfModelFolders.Count)
		//	{
		//		StartCoutned = true;
		//	}

			var buttonObject = Instantiate(buttonPrefab);
			var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
			//var ModelVal = buttonObject.GetComponent<modelValueInButton>();



			buttonText.text = Name + count.ToString();


			buttonObject.transform.SetParent(parent.transform);
			buttonObject.transform.localScale = Vector3.one;
			buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);

			modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
			modelValueInButton.modelVal = downloadHandler.ListOfModelFolders.Count;
			modelValueInButton.downloadHandler = downloadHandler;


	//	}
	}
}
