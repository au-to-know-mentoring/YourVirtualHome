using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using OculusSampleFramework;
using TMPro;
using UnityEngine;

public class modelValueInButton : MonoBehaviour
{
	public string modelPath;
	public DownloadHandler  downloadHandler;
    public int modelVal;
	private GameObject controlPanel;

	public TMP_Text FirstName;
	public TMP_Text ClientName;

	private bool showmodelPathBool;

	private void Start() {
		downloadHandler = FindObjectOfType<DownloadHandler>();
		
	}
	public void showModelPath()
	{
		//downloadHandler.modelSelectInt = modelVal;
	}
	public void ShowApprovementPanel()
	{
		ApprovementPanel.instance._modelValueInButton = this;

		ApprovementPanel.instance.myPanel.SetActive(true);
	}

	/// <summary>
	/// Handles deltion of model button from the scroll view and file
	/// </summary>
	public void DeleteModel()
	{
		downloadHandler.DeleteModel(modelPath);


		Destroy(this.gameObject);
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
	IEnumerator ShowLoadingPanel(){
		if (downloadHandler.canvas.enabled != true){
			downloadHandler.canvas.enabled = true;
			// importModel();
		}
		yield return new WaitForSeconds(0.1f);
		
		downloadHandler.LoadModelToScene(modelVal);
        FindObjectOfType<SettingsSceneManager>().SetupHouseDummy();
        FindObjectOfType<SettingsScenePlayer>().modelVal = modelVal;
	}
	public void importModel()
	{
		if(DataManager.Instance.GetHouse() != null){
			Destroy(DataManager.Instance.GetHouse());
		}

		StartCoroutine(ShowLoadingPanel());
		

		

		showControlPanel();
		Debug.Log(modelVal);
	}
}
