using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportModelToModelViewScene : MonoBehaviour
{
    private DownloadHandler DownloadHandlerScript;
	public GameObject Player;
	public GameObject spawnHere;

	// Start is called before the first frame update

	void Start()
	{
		
		DownloadHandlerScript = gameObject.GetComponent<DownloadHandler>();
		DownloadHandlerScript.ModelHolderParent = spawnHere;
		DownloadHandlerScript.choose = DownloadHandlerScript.modelSelectInt;
		DownloadHandlerScript.LoadModelToScene();
	}
	void Update()
	{
		var modelSelectInt = DownloadHandlerScript.choose;
		Debug.Log(modelSelectInt);


	}


}
