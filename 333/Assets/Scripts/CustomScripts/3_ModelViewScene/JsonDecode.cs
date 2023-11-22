using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDecode : MonoBehaviour
{
	public TextAsset JsonText;
	public GetModelName myGetModelNameScript;
	public string modelInfoForString;

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
		string ModelInfoFromServer = myGetModelNameScript.ModelInfo + "] }";
		modelInfoForString = jsonStartName + ModelInfoFromServer;
		myModelList = JsonUtility.FromJson<ModelList>(jsonStartName + ModelInfoFromServer);
	}
}
