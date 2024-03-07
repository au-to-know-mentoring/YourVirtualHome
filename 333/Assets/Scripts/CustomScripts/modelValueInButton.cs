using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class modelValueInButton : MonoBehaviour
{
	public DownloadHandler downloadHandler;
    public int modelVal;
	private GameObject controlPanel;

	public TMP_Text FirstName;
	public TMP_Text ClientName;

	private bool showmodelPathBool;
	public void showModelPath()
	{
		//downloadHandler.modelSelectInt = modelVal;
	}
	public void showControlPanel()
	{
		ControlPanelShow controlPanel = FindObjectOfType<ControlPanelShow>();
		controlPanel.showControlPanel();
	}

	public void setSettingsScenePlayerModelVal()
	{
		FindObjectOfType<SettingsScenePlayer>().getModelIntFromUIButton(gameObject);
	}

	public void importModel()
	{
		showControlPanel();
		Debug.Log(modelVal);

		FindObjectOfType<DownloadHandler>().LoadModelToScene(modelVal);
		FindObjectOfType<SettingsSceneManager>().SetupHouseDummy();
		FindObjectOfType<SettingsScenePlayer>().modelVal = modelVal;
		
	}
}
