using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetModelFromDownloadHandler : MonoBehaviour
{
    private DownloadHandler DownloadhandlerScript;
    public GameObject DownloadHandlerOBJ;
    private TMP_Dropdown modelSelectDropDown;
    private int myList;
    private int myChoice;
    // Start is called before the first frame update
    void Start()
    {
        DownloadhandlerScript = DownloadHandlerOBJ.GetComponent<DownloadHandler>();
        modelSelectDropDown = gameObject.GetComponent<TMP_Dropdown>();
        myChoice = DownloadhandlerScript.choiceTest;
        
    }
    public void TestChangeModel()
    {
        //get the selected index
        int menuIndex = modelSelectDropDown.GetComponent<TMP_Dropdown>().value;

        //get all options available within this dropdown menu
        List<TMP_Dropdown.OptionData> menuOptions = modelSelectDropDown.GetComponent<TMP_Dropdown>().options;

        //get the string value of the selected index
        string value = menuOptions[menuIndex].text;
        
        if (menuIndex == 1)
		{
            myChoice = 69;
		}

        Debug.Log(menuIndex);
        
            
    }
    // Update is called once per frame
    void Update()
    {
        var myList = DownloadhandlerScript.TEST;
        TestChangeModel();
        
    }
}
