using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Keyboard : MonoBehaviour
{
    private Collider InputFieldCollider;
    public InputField iField;
    public Text Canvastext;
    public GameObject Controller;
    private TouchScreenKeyboard overlayKeyboard;
    public GameObject ViewableCyclinder;
    public static string inputText = "";
    private RaycastHit hit;
    private bool isVisible;
    private float UiDistance;
   
    public DownloadHandler DownloadHandlerObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void RaycastInputField()
    {
        //      Vector3 rayOrigin = Controller.transform.position;
        //      RaycastHit hit;

        //      Debug.Log("Running");
        //      //Debug.DrawLine(rayOrigin, hit.collider.gameObject.transform.position, Color.white);
        //      Debug.DrawRay(rayOrigin, Controller.transform.forward, Color.red);

        //      if (Physics.Raycast(rayOrigin, Controller.transform.forward, out hit, Mathf.Infinity)) // Check if our raycast has hit anything
        //      {


        //          //Debug.Log("hit: " + hit.collider.gameObject.tag);

        //	if (hit.collider.gameObject.tag == "InputField")
        //	{
        //              Debug.Log("hit" + hit.collider.gameObject.tag);
        //		// ViewableCyclinder.active = true;



        //	}
        //}

        RaycastHit[] hits;
        Vector3 direction = Controller.transform.forward;
        hits = Physics.RaycastAll(Controller.transform.position, direction, Mathf.Infinity);
        if (hits.Length < 1) {
            ViewableCyclinder.active = false;
            return; }

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Hit Lenghth: " + hits.Length);
            Debug.Log(hits[i].transform.gameObject.name);
            if (hits[i].transform.gameObject.tag == "InputField")
            {


				if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
				{
					Debug.Log("trigger Pressed");
					ShowKeyboard();

				}


			} else  if (hits[i].transform.gameObject.tag == "DownloadButton")
				{
                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
					DownloadHandlerObject.DownloadFile();
                    }
				}
			 else if (hits[i].transform.gameObject.tag == "GoToScene")
			{
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
					DownloadHandlerObject.SwitchToModelScene();
                }
			}else if (hits[i].transform.gameObject.tag == "ChooseModelTest")
            {
				if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
				{
                    DownloadHandlerObject.choiceTest = 5;
				}
			}
			
		

			if (hits[i].transform.gameObject.layer == 5)
            {
                ViewableCyclinder.active = true;
                Debug.Log("UiDistance" + UiDistance);
            } 


        } 
        
    }
    public void ShowKeyboard()
	{
        Debug.Log("Trigger Pressed");
        iField = iField.GetComponent<InputField>();
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        
        if (overlayKeyboard != null)
        {
            Debug.Log("true");
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (DownloadHandlerObject != null)
        //{
        //    Debug.Log("does have DownloadHandlerScript");
        //}
		

		RaycastInputField();
        iField.text = overlayKeyboard.text;
        Canvastext.text = iField.text;
		
	}
}
