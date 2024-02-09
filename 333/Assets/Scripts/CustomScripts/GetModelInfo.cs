using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetModelInfo : MonoBehaviour
{

	private DownloadHandler downloadHandler;

	[SerializeField] private TMP_InputField iField;


	public string RequestInfo;


	public string Return;
	private void Start()
	{
		downloadHandler = FindObjectOfType<DownloadHandler>();
	}
	public void getModelInfo(string Code, string key)
	{
		var coroutine = ServerResonce(Code, key);
		StartCoroutine(coroutine);
	}
	public IEnumerator ServerResonce(string Code, string key)
	{
		var ModelInfoRequest = new WWW("https://aumentoring.com.au/virtualhome-remote/getModelInfo/" + Code);

		yield return ModelInfoRequest;

		RequestInfo = ModelInfoRequest.text;
		Debug.Log(RequestInfo);
		RunJsonDecode();



		// used to add the Name and Client Name to the button in the Model scrollview
		Return = "Name: " + myModelList.Modelinfo[0].Name + " " + "Client Name: " + myModelList.Modelinfo[0].Client;
		PopulateScrollView myPopView = FindObjectOfType<PopulateScrollView>();
		myPopView.AddModelButton(key, Return);
		
		
	}

	[System.Serializable]
	public class ModelInfo
	{
		public string Name;
		public string Client;
	}
	[System.Serializable]
	public class ModelList
	{
		public ModelInfo[] Modelinfo;

	}
	public ModelList myModelList = new ModelList();
	public void RunJsonDecode()
	{
		string jsonStartName = "{" + "\"" + "Modelinfo" + "\"" + ": [";
		string PlayerLoginInfoFromServer = RequestInfo + "] }";


		myModelList = JsonUtility.FromJson<ModelList>(jsonStartName + PlayerLoginInfoFromServer);

		
	}
}