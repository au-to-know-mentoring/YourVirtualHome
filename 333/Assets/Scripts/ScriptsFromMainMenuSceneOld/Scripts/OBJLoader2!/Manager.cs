/* using UnityEngine;
using System.Collections;
using System.IO;

public class Manager : MonoBehaviour {

	public GameObject model;

	void Start () 
	{

		ObjectLoader loader = model.AddComponent <ObjectLoader> ();
		loader.Load(Application.dataPath + "/Resources/", "Test1.obj");
	}
}
*/
