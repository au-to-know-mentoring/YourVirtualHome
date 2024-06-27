using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprovementPanel : MonoBehaviour
{
    public static ApprovementPanel instance;

    private void Awake() {
        if (instance == null){
            instance = this;
        }else {
            Destroy(this);
        }
    }

    public modelValueInButton _modelValueInButton;

    public GameObject myPanel;

    public void Hide(){
        myPanel.SetActive(false);
    }
    public void Delete()
    {
        _modelValueInButton.DeleteModel();
    }
    public void Select(){
        _modelValueInButton.importModel();
        // _modelValueInButton.showControlPanel();
    }

    
    
}
