using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModelInfo : MonoBehaviour
{
	public DownloadHandler downloadHandler;
	private string modelInfoForString = "Modelinfo";


	public string RequestInfo;
	public string clientName;
	public string modelName;
	public void getModelInfo()
	{
		var coroutine = ServerResonce();
		StartCoroutine(coroutine);
	}
	public IEnumerator ServerResonce()
	{
		var ModelInfoRequest = new WWW("http://localhost:3000/virtualhome-remote/getModelInfo/" + downloadHandler.myInput);

		yield return ModelInfoRequest;

		RequestInfo = ModelInfoRequest.text;
		Debug.Log(RequestInfo);
		RunJsonDecode();
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
		string jsonStartName = "{" + "\"" + modelInfoForString + "\"" + ": [";
		string ModelInfoFromServer = RequestInfo + "] }";
		myModelList = JsonUtility.FromJson<ModelList>(jsonStartName + ModelInfoFromServer);
	}
}
