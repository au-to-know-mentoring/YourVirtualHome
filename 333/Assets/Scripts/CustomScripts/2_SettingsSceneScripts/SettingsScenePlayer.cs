using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsScenePlayer : MonoBehaviour
{
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

    private void Start()
    {
        rightX.action.Enable();
        rightX.action.performed += GrabButton;
        rightX.action.canceled += GrabRelease;
        rightTrigger.action.Enable();
        rightTrigger.action.performed += ResetButton;

        lr.positionCount = 2;
        aimDirection = Vector3.forward;
        lr.SetPosition(1, aimDirection * 20);

        spawnerIndicator = Instantiate(spawnerIndicatorPrefab, Vector3.zero, Quaternion.identity);
        spawnerIndicator.SetActive(false);
    }

    private void GrabButton(InputAction.CallbackContext context) {
        placeSpawner();
        PressButton();

        lr.positionCount = 2;
        lr.SetPosition(1, aimDirection*20);
    }
    private void GrabRelease(InputAction.CallbackContext context) {
        sliderDragging = false;
        DataManager.Instance.SetHouse(dollhouse);
    }
    private void ResetButton(InputAction.CallbackContext context) {
        //dollhouse.transform.eulerAngles = Vector3.zero;
        ResetSpawnIndicator();
    }

    void Update()
    {
        DragSlider();
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

    void PressButton()
    {
        RaycastHit[] hits;
        Vector3 direction = controller.transform.TransformDirection(aimDirection);
        hits = Physics.RaycastAll(controller.transform.position, direction, Mathf.Infinity);
        if (hits.Length < 1) { return; }

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].transform.gameObject.name == "Handle")
            {
                sliderDragging = true;
                break;
            }
            if (hits[i].transform.gameObject.GetComponent<Button>() != null) {
                hits[i].transform.gameObject.GetComponent<Button>().onClick.Invoke();
                break;
            }
        }
    }

    public void EnableSpawnerPlacement() {
        placingSpawner = true;
    }

    private void PositionSpawner() {
        if (!placingSpawner)
            return;
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
                Debug.Log(Vector3.Angle(Vector3.up, hits[i].normal));
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
        DataManager.Instance.SetSpawnPosition(spawnerIndicator.transform.position);
    }
    private void ResetSpawnIndicator()
    {
        placingSpawner = false;
        ChangeLineRendererColor(Color.red);
        spawnerIndicator.transform.position = DataManager.Instance.GetSpawnPosition();
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
