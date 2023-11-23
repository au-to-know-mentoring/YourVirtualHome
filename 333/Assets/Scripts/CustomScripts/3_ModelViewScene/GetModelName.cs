using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GetModelName : MonoBehaviour
{
    public Text nameText;
    public Text clientNameText;

    public string clientName;
    public string modelName;

    public DownloadHandler downloadHandler;

    public JsonDecode myJsonDecodeScript;

    public string ModelInfo;
     void Start()
	{
		getModelInfo();
	}
	public void getModelInfo()
    {
		var coroutine = testResonce();
		StartCoroutine(coroutine);
	}
	public IEnumerator  testResonce()
	{

        var ModelName = new WWW("http://localhost:3000/virtualhome-remote/getModelInfo/746241");
		// var ClientName = new WWW("http://localhost:3000/virtualhome-remote/getModelInfo/" + downloadHandler.myInput + "?clientname=");
		/* wait for the download of the response to complete */
		

		// yield return ClientName;
		yield return ModelName;
        /* display the content from the response */
        
        Debug.Log(ModelName.text);
      //  Debug.Log(ClientName.text);
        //clientName = ClientName.text;
        name = ModelName.text;
        ModelInfo = ModelName.text;
      //  clientNameText.text = ClientName.text;
        nameText.text = ModelName.text;
       // clientName = ClientName.text;
        modelName = ModelName.text;
        myJsonDecodeScript.RunJsonDecode();
	}
	
   

}
