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

    [SerializeField] GameObject TrackingAreaVr;
    [SerializeField] GameObject CameraRigVR;

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
  
        // rightTrigger.action.performed += ResetButton;

        lr.positionCount = 2;
        aimDirection = Vector3.forward;
        lr.SetPosition(1, aimDirection * 20);

        spawnerIndicator = Instantiate(spawnerIndicatorPrefab, Vector3.zero, Quaternion.identity);
        spawnerIndicator.SetActive(false);
    }

    private void GrabButton(InputAction.CallbackContext context) {
        placeSpawner();
     
        

    
    }
  
    private void ResetButton(InputAction.CallbackContext context) {
        //dollhouse.transform.eulerAngles = Vector3.zero;
        ResetSpawnIndicator();
    }

    void Update()
    {
        PositionSpawner();
    }

   
    



    public void ShowKeyboard(TMP_InputField iField)
    {
        iField.text = overlayKeyboard.text;
		overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    public void applyDownloadCode(InputField iField)
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
    IEnumerator TeleportToSpawn(){
        yield return new WaitForSeconds(0.025f);
        CameraRigVR.transform.position = DataManager.Instance.GetSpawnPosition().transform.position;

    }

    public void SwitchTeleportControllerOn()
    {
        

        MainMenuCanvas.active = false;
        ControlPanelCanvas.active = false;

		saveModelRotation();

        



		var House = FindObjectOfType<DataManager>().GetHouse();
        Vector3 HouseTransform = new Vector3(House.transform.rotation.x, House.transform.rotation.y, House.transform.rotation.z);
       
        House.transform.parent = null;
        House.transform.localScale /= 0.025f;
        // House.transform.position += new Vector3(0, 0, 0);

        //  PlayerPrefs.SetFloat( "ModelX" + modelVal, House.transform.rotation.x);
		//  PlayerPrefs.SetFloat( "ModelZ" + modelVal, House.transform.rotation.y);
		//  PlayerPrefs.SetFloat( "ModelZ" + modelVal, House.transform.rotation.z);

   
        
        // CameraRigVR.transform.position = spawnerIndicator.transform.position;

        // CameraRigVR.transform.position = TrackingAreaVr.transform.position;

        // TrackingAreaVr.transform.SetParent(null);

        StartCoroutine(TeleportToSpawn());

        CustomTeleporter myTP = FindObjectOfType<CustomTeleporter>();
        myTP.enabled = true;

        Wand myWand = FindObjectOfType<Wand>();
        myWand.enabled = true;
        // TrackingAreaVr.transform.position = CameraRigVR.transform.position;
        
        // spawnerIndicator.gameObject.SetActive(false);


        
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
        DataManager.Instance.SetSpawnPosition(spawnerIndicator);
		rightTrigger.action.Disable();

		spawnerIndicator.transform.SetParent(DataManager.Instance.GetHouse().transform);
       
    }
    public void ResetSpawnIndicator()
    {
        placingSpawner = false;
        ChangeLineRendererColor(Color.red);
        spawnerIndicator.transform.position = DataManager.Instance.GetSpawnPosition().transform.position;
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
