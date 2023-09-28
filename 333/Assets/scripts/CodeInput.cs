using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeInput : MonoBehaviour
{
    public InputField iField;
    string myInput;
   public string link = "http://localhost:3000/virtualhome-remote/getModel/";
    
   //public string linkPlusInput = myInput + link;
   
       // public GameObject inputField;
    // public TMPro = inputField.GetComponent<TMP_InputField>;

    //Debug.log(text);
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  

    public void attachCodeToLink()
    {
         string linkPlusInput = myInput + link;
        myInput = iField.text;

        Debug.Log(myInput);


        Debug.Log(linkPlusInput + myInput);
    }
   public void openLink() 
   {
    myInput = iField.text;
    string linkPlusInput = link + myInput;
    
    if (myInput != null) {
        Application.OpenURL(linkPlusInput);
        Debug.Log(linkPlusInput);
    }
    
   }
}
