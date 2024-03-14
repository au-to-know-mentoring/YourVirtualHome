using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsScenePlayer : MonoBehaviour
{

    public int modelVal;

    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ControlPanelCanvas;

    private TMP_InputField CodeInputField;

    [SerializeField] GameObject VrRigParent;

    public TMP_Text myText;

    private TMP_InputField myIField;

    [HideInInspector] public int ModelVal;

    //private TMP_InputField iField;
    private TouchScreenKeyboard overlayKeyboard;

    public DownloadHandler myDownloadHandler;

    [Header("Controller")]
    [SerializeField] InputActionReference rightX;
    [SerializeField] InputActionReference rightTrigger;
    [SerializeField] GameObject controller;

    //public OVRInput.Button grabButton;
    //public OVRInput.Button resetRotationButton;
    private Vector3 aimDirection;
    [SerializeField] LineRenderer lr;

    [Header("Dollhouse")]
    public GameObject dollhouse;

    [Header("Canvas Settings")]
    public float activationDistance;
    private bool sliderDragging = false;
    [SerializeField] Transform Slider0Point;
    [SerializeField] Scrollbar slider;
    // spawner related variables
    private bool placingSpawner = false;
    [SerializeField] GameObject spawnerIndicatorPrefab;
    GameObject spawnerIndicator;
    float maxNormalAngle = 45f;
    bool canPlaceSpawner = false;

    [Header("MainMenuCanvas")]
    [SerializeField] Transform VSlider0Point;
    [SerializeField] Transform VSlider100point;
    [SerializeField] Scrollbar VSlider;
    private bool VSliderDragging = false;

    private void Start()
    {



        rightX.action.Enable();
        rightX.action.performed += GrabButton;
        //rightX.action.canceled += GrabRelease;
        rightTrigger.action.performed += ResetButton;

        lr.positionCount = 2;
        aimDirection = Vector3.forward;
        lr.SetPosition(1, aimDirection * 20);

        spawnerIndicator = Instantiate(spawnerIndicatorPrefab, Vector3.zero, Quaternion.identity);
        spawnerIndicator.SetActive(false);
    }

    private void GrabButton(InputAction.CallbackContext context) {
        placeSpawner();
       // PressButton();
        //PressButtonDiff();
        

        lr.positionCount = 2;
        lr.SetPosition(1, aimDirection * 20);
    }
    private void GrabRelease(InputAction.CallbackContext context) {
        sliderDragging = false;
        VSliderDragging = false;
        //DataManager.Instance.SetHouse(dollhouse);
    }
    private void ResetButton(InputAction.CallbackContext context) {
        //dollhouse.transform.eulerAngles = Vector3.zero;
        ResetSpawnIndicator();
    }

    void Update()
    {
		//if (overlayKeyboard != null)
		//{
		//	CodeInputField.text = overlayKeyboard.text;
		//	Debug.Log("true");
		//}

		//DragSlider();
        //DragSliderVertical();

        PressButton();

        PositionSpawner();
    }

    void DragSlider() {
        if (sliderDragging) {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(controller.transform.position, controller.transform.TransformDirection(aimDirection), Mathf.Infinity);
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.gameObject.name == "SlidingArea") {
                    float dist = Vector3.Distance(hits[i].point, Slider0Point.position);
                    if (dist <= 0)
                    {
                        slider.value = 0;
                    }
                    else if (dist >= 6)
                    {
                        slider.value = 1;
                    }
                    else {
                        dist = Mathf.Clamp(dist, 0, 6);
                        slider.value = 1 - Remap(dist, 0, 6, 0, 1); // inverting the number
                    }
                }
            }
        }
    }

    void DragSliderVertical()
    {
        if (VSliderDragging)
        {
            //Debug.Log("VSlider  dragging");
            RaycastHit[] hits;
            hits = Physics.RaycastAll(controller.transform.position, controller.transform.TransformDirection(aimDirection), Mathf.Infinity);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.name == "VSlidingArea")
                {
                    //Debug.Log("VSlider moving");
                    float dist = Vector3.Distance(hits[i].point, VSlider0Point.position);
                    float maxDist = Vector3.Distance(VSlider100point.position, VSlider0Point.position);
                    if (dist <= 0)
                    {
                        VSlider.value = 1;
                    }
                    else if (hits[i].point.y > VSlider0Point.position.y)
                    {
                        VSlider.value = 1;
                    }
                    else if (dist >= maxDist)
                    {
                        VSlider.value = 0;
                    }
                    else
                    {
                        dist = Mathf.Clamp(dist, 0, maxDist);
                        VSlider.value = 1 - Remap(dist, 0, maxDist, 0, 1); // inverting the number
                    }
                }
            }
        }
    }

    void PressButton()
    {
        RaycastHit[] hits;
        Vector3 direction = controller.transform.TransformDirection(aimDirection);
        hits = Physics.RaycastAll(controller.transform.position, direction, Mathf.Infinity);
        if (hits.Length < 1) { return; }


            for (int i = 0; i < hits.Length; i++) {
            if (rightX.action.WasPressedThisFrame() == true) {
                if (hits[i].transform.gameObject.GetComponent<Button>() != null)
                {   
                    hits[i].transform.gameObject.GetComponent<Button>().onClick.Invoke();    
                    break;
                }

                if (hits[i].transform.gameObject.GetComponent<TMP_InputField>() != null)
                {
                    hits[i].transform.gameObject.GetComponent<TMP_InputField>().ActivateInputField();
                    hits[i].transform.gameObject.GetComponent<TMP_InputField>().Select(); 
                    myIField.gameObject.GetComponent<TMP_InputField>().DeactivateInputField();
                    break;
                }
            }
        }
    }
    TMP_InputField myInputField;
    // void OnApplicationFocus(bool hasFocus)
    // {
    //     myInputField.Select()
    // }
    void PressButtonDiff()
    {

        RaycastHit hits;
        Vector3 direction = controller.transform.TransformDirection(aimDirection);
        if(Physics.Raycast(controller.transform.position, direction, out hits,Mathf.Infinity))
        {
            Debug.Log(hits.transform.gameObject.name);
                if (hits.transform.gameObject.GetComponent<Button>() != null)
                {   
                    hits.transform.gameObject.GetComponent<Button>().onClick.Invoke();    
                }

                else if (hits.transform.gameObject.GetComponent<TMP_InputField>() != null)
                {


                    //hits.transform.gameObject.GetComponent<TMP_InputField>().Select(); 
                }
            
        }
    }

    



    public void ShowKeyboard(TMP_InputField iField)
    {
        iField.text = overlayKeyboard.text;
		overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    public void applyDownloadCode(TMP_InputField iField)
    {
        myDownloadHandler.DownloadFile(iField.text);
        //printText(iField.text);
    }

    private void printText(string inputText)
    {
        myText.text = inputText;
    }

    public void getModelIntFromUIButton(GameObject myButton)
    {
        ModelVal = myButton.GetComponent<modelValueInButton>().modelVal;
    }

    public void SwitchTeleportControllerOn()
    {
        

        MainMenuCanvas.active = false;
        ControlPanelCanvas.active = false;

		saveModelRotation();

        CustomTeleporter myTP = FindObjectOfType<CustomTeleporter>();
        myTP.enabled = true;

        Wand myWand = FindObjectOfType<Wand>();
        myWand.enabled = true;



		var House = FindObjectOfType<DataManager>().GetHouse();
        Vector3 HouseTransform = new Vector3(House.transform.rotation.x, House.transform.rotation.y, House.transform.rotation.z);
       
        House.transform.parent = null;
        House.transform.localScale /= 0.025f;
        House.transform.position += new Vector3(10f, 0, 0);

         PlayerPrefs.SetFloat( "ModelX" + modelVal, House.transform.rotation.x);
		 PlayerPrefs.SetFloat( "ModelZ" + modelVal, House.transform.rotation.y);
		 PlayerPrefs.SetFloat( "ModelZ" + modelVal, House.transform.rotation.z);



        VrRigParent.transform.position = DataManager.Instance.GetSpawnPosition().position;
        spawnerIndicator.gameObject.SetActive(false);

        if (PlayerPrefs.GetString("modelSettings" + ModelVal) != "")
        {
            House.transform.rotation = LoadModelWithSettingsApplied();
        }

        
	}

	#region Model rotation/ PlayerPref
	public void saveModelRotation()
    {
		PlayerPrefs.SetString("modelSettings" + ModelVal, new Vector3(FindObjectOfType<DataManager>().GetHouse().transform.rotation.x, FindObjectOfType<DataManager>().GetHouse().transform.rotation.y, FindObjectOfType<DataManager>().GetHouse().transform.rotation.z).ToString());
	}

	public Quaternion LoadModelWithSettingsApplied()
    {
		Quaternion myQuat = new Quaternion();
		
		Vector3 myVector3 = StringToVector3("(" + PlayerPrefs.GetString("modelSettings" + ModelVal) + ")");
        Debug.Log(myVector3.ToString());
		myQuat.x = myVector3.x;
		myQuat.y = myVector3.y;
		myQuat.z = myVector3.z;
		return myQuat;
	}

	public static Vector3 StringToVector3(string sVector)
	{
		// Remove the parentheses
		if (sVector.StartsWith("(") && sVector.EndsWith(")"))
		{
			sVector = sVector.Substring(1, sVector.Length - 2);
		}

		// split the items
		string[] sArray = sVector.Split(',');

		// store as a Vector3
		Vector3 result = new Vector3(
			float.Parse(sArray[0]),
			float.Parse(sArray[1]),
			float.Parse(sArray[2]));

		return result;
	}
	#endregion

	public void EnableSpawnerPlacement() { 
        placingSpawner = true;
    }

    private void PositionSpawner() {
        if (!placingSpawner)
            return;
		rightTrigger.action.Enable();
		// use tags or layermask to check if house?
		spawnerIndicator.SetActive(false);
        ChangeLineRendererColor(Color.red);
        canPlaceSpawner = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(controller.transform.position, controller.transform.TransformDirection(aimDirection), Mathf.Infinity);
        if (hits.Length > 0) {
            // organise hits from closest to farthest
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.gameObject.tag == "SettingsCanvas") {
                    continue;
                }
                //Debug.Log(Vector3.Angle(Vector3.up, hits[i].normal));
                if (Vector3.Angle(Vector3.up, hits[i].normal) < maxNormalAngle)
                {
                    ChangeLineRendererColor(Color.green);
                    spawnerIndicator.SetActive(true);
                    spawnerIndicator.gameObject.transform.position = hits[i].point;
                    canPlaceSpawner = true;
                    break;
                }
            }
        }
    }
    private void placeSpawner() {
        if (!canPlaceSpawner)
            return;

        placingSpawner = false;
        ChangeLineRendererColor(Color.red);
        DataManager.Instance.SetSpawnPosition(spawnerIndicator.transform);
		rightTrigger.action.Disable();

		spawnerIndicator.transform.SetParent(FindObjectOfType<DataManager>().GetHouse().transform);
       
    }
    public void ResetSpawnIndicator()
    {
        placingSpawner = false;
        ChangeLineRendererColor(Color.red);
        spawnerIndicator.transform.position = DataManager.Instance.GetSpawnPosition().position;
        spawnerIndicator.SetActive(true);
    }

    // utility functions-------------------
    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    private void ChangeLineRendererColor(Color color)
    {
        lr.startColor = color;
        lr.endColor = color;
    }
}
