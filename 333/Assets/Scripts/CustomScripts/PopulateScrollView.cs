using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.ComponentModel;
using System;

public class PopulateScrollView : MonoBehaviour
{
    public DownloadHandler downloadHandler;
	public GameObject parent;
	public GameObject buttonPrefab;
	private int count = 0;
	
	private void Update()
	{
		
		if (count < downloadHandler.ListOfModelFolders.Count)
		{
			count++;
			var buttonObject = Instantiate(buttonPrefab);
			var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
			//var ModelVal = buttonObject.GetComponent<modelValueInButton>();
			
			

			buttonText.text = "string" + count.ToString();

			
			buttonObject.transform.SetParent(parent.transform);
			buttonObject.transform.localScale = Vector3.one;
			buttonObject.transform.position = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);
			
			buttonObject.GetComponent<RectTransform>().localPosition = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 0f);
			
			modelValueInButton modelValueInButton = buttonObject.GetComponent<modelValueInButton>();
			modelValueInButton.modelVal = downloadHandler.ListOfModelFolders.Count;
			modelValueInButton.downloadHandler = downloadHandler;

			
		}
	}
}
