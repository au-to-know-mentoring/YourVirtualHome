using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsScenePlayer : MonoBehaviour
{

    
    [SerializeField] private PlayerSetup playerSetup;

    [SerializeField] GameObject CameraRigVR;

    public TMP_Text myText;

    [HideInInspector] public int ModelVal;


    public DownloadHandler myDownloadHandler;

    [Header("Controller")]
    [SerializeField] InputActionReference rightX;
    [SerializeField] InputActionReference rightTrigger;
    [SerializeField] InputActionReference aButton;
    [SerializeField] GameObject controller;

    //public OVRInput.Button grabButton;
    //public OVRInput.Button resetRotationButton;
    private Vector3 aimDirection;
    [SerializeField] LineRenderer lr;

    public GameObject customTpObj;

    public bool inModelScene;



    // spawner related variables
    private bool placingSpawner = false;
    [SerializeField] GameObject spawnerIndicatorPrefab;
    GameObject spawnerIndicator;
    float maxNormalAngle = 45f;
    bool canPlaceSpawner = false;


    private CustomTeleporter customTeleporter;

   
    [Header("Controls Info Panel")]


    public ControlsInfo controlsInfo;
    public InputField codeFieldForControlsInfo;

    private void Start()
    {
        inModelScene = false;
        customTeleporter = FindObjectOfType<CustomTeleporter>();

        if (placingSpawner == true){
            customTpObj.SetActive(false);
            var myTP = FindObjectOfType<CustomTeleporter>();
            myTP.enabled = false;
        }

        rightX.action.Enable();
        rightX.action.performed += GrabButton;
        
        rightTrigger.action.Enable();
        rightTrigger.action.performed += GrabButton;

        aButton.action.Enable();
        aButton.action.performed += TurnOffTeleporter;



        // rightTrigger.action.performed += ResetButton;

        lr.positionCount = 2;
        aimDirection = Vector3.forward;
        lr.SetPosition(1, aimDirection * 20);

        spawnerIndicator = Instantiate(spawnerIndicatorPrefab, Vector3.zero, Quaternion.identity);
        spawnerIndicator.SetActive(false);
    }

    private void GrabButton(InputAction.CallbackContext context)
    {
        placeSpawner();
    }
    private void TurnOffTeleporter(InputAction.CallbackContext context)
    {
        var myTP = FindObjectOfType<CustomTeleporter>();
        myTP.enabled = false;
    }

    private void ResetButton(InputAction.CallbackContext context)
    {
        //dollhouse.transform.eulerAngles = Vector3.zero;
        ResetSpawnIndicator();
    }

    void Update()
    {
        PositionSpawner();
        codeFieldForControlsInfo.onValueChanged.AddListener(delegate {TurnOnControlsInfo(); });
    }

    public void TurnOnControlsInfo()
    {
        controlsInfo.enabled = false;
        
    }

    public void applyDownloadCode(InputField iField)
    {

        if (iField.text == ""){
            iField.Select();
        }else {
            myDownloadHandler.ICode = iField.text;
            myDownloadHandler.CallCheckCodeStatus(iField.text);
        }
        controlsInfo.enabled = true;
        
        //printText(iField.text);
    }

    private void printText(string inputText)
    {
        myText.text = inputText;
    }

    public void getModelIntFromUIButton(int modelValue)
    {
        ModelVal = modelValue;
    }
    IEnumerator TeleportToSpawn()
    {
        yield return new WaitForSeconds(0.025f);
        CameraRigVR.transform.position = DataManager.Instance.GetSpawnPosition().transform.position;

    }

    public void SwitchTeleportControllerOn()
    {



        FindObjectOfType<SettingsScenePlayer>().inModelScene = true;
        // saveModelRotation();
        aButton.action.Disable();

        var House = FindObjectOfType<DataManager>().GetHouse();

        ModelSettings.Instance.SaveSettings(ModelVal.ToString(), FindObjectOfType<SettingsSceneManager>().dollhouseParent.transform.position.ToString(), FindObjectOfType<SettingsSceneManager>().dollhouseParent.transform.rotation.ToString());

        House.transform.parent = null;
        House.transform.localScale /= 0.025f;

        playerSetup.SettingUpPlayerForModelScene();

        StartCoroutine(TeleportToSpawn());
        spawnerIndicator.gameObject.SetActive(false);

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

    public void EnableSpawnerPlacement()
    {
        placingSpawner = true;
    }

    private void PositionSpawner()
    {
        if (!placingSpawner)
            return;
        rightTrigger.action.Enable();
        // use tags or layermask to check if house?
        spawnerIndicator.SetActive(false);
        ChangeLineRendererColor(Color.red);
        canPlaceSpawner = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(controller.transform.position, controller.transform.TransformDirection(aimDirection), Mathf.Infinity);
        if (hits.Length > 0)
        {
            // organise hits from closest to farthest
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.tag == "SettingsCanvas")
                {
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
    private void placeSpawner()
    {
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
