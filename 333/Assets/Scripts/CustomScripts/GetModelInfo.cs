using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetModelInfo : MonoBehaviour
{

	public static GetModelInfo Instance;

	private void Awake() {
		if (Instance == null){
			Instance = this;
		}else {
			Destroy(this);
		}
	}

	private DownloadHandler downloadHandler;

	[SerializeField] private TMP_InputField iField;


	// public string RequestInfo;


	public string Return;
	private void Start()
	{
		downloadHandler = FindObjectOfType<DownloadHandler>();
	}


	/// <summary>
	/// Saves JsonEncode to modelpath/JsonEncode.txt    
	/// aswell as populates the scrollview with a new model button
	/// </summary>
	/// <param name="Code">code input for "https://aumentoring.com.au/virtualhome-remote/getModelInfo/" </param>
	/// <param name="path">modelpath/JsonEncode.txt must include the .txt file</param>
	public IEnumerator SaveJsonRequest(string Code, string path)
	{
		var ModelInfoRequest = new WWW("https://aumentoring.com.au/virtualhome-remote/getModelInfo/" + Code);

		yield return ModelInfoRequest;
		
		File.WriteAllText(path, ModelInfoRequest.text);
		


		RunJsonDecode(ModelInfoRequest.text);


	
	
		PopulateScrollView myPopView = FindObjectOfType<PopulateScrollView>();

		myPopView.AddModelButton(myModelList.Modelinfo[0].Name, myModelList.Modelinfo[0].Client);
		
	}

	/// <summary>
	/// Populates the scroll view with model buttons, with already downloaded models
	/// </summary>
	/// <param name="jsonEncoded">there should be a modelpath/JsonEncode.txt in the models unzip folder</param>
	/// <param name="modelcount">modelcount is just an int value for the buttons modelval</param>
	public void RunJsonDecodeForModelButton(string jsonEncoded, int modelcount){

		RunJsonDecode(jsonEncoded);
		PopulateScrollView myPopView = FindObjectOfType<PopulateScrollView>();

		myPopView.AddModelButtonOnStart(myModelList.Modelinfo[0].Name, myModelList.Modelinfo[0].Client, modelcount);
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
	public void RunJsonDecode(string RequestInfo)
	{
		string jsonStartName = "{" + "\"" + "Modelinfo" + "\"" + ": [";
		string PlayerLoginInfoFromServer = RequestInfo + "] }";


		myModelList = JsonUtility.FromJson<ModelList>(jsonStartName + PlayerLoginInfoFromServer);

		
	}
}