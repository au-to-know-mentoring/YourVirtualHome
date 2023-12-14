using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class modelValueInButton : MonoBehaviour
{
	public DownloadHandler downloadHandler;
    public int modelVal;
	public GameObject controlPanel;

	private bool showmodelPathBool;
	public void showModelPath()
	{
		downloadHandler.modelSelectInt = modelVal;
	}
	public void showControlPanel()
	{
		ControlPanelShow controlPanel = FindObjectOfType<ControlPanelShow>();
		controlPanel.showControlPanel();
	}
	public void importModel()
	{
		showControlPanel();

		FindObjectOfType<DownloadHandler>().LoadModelToScene();
		FindObjectOfType<SettingsSceneManager>().SetupHouseDummy();
		
	}
}
