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

    [Header("Activation Settings")]
    public float activationDistance;
    private bool sliderDragging = false;
    [SerializeField] Transform Slider0Point;
    [SerializeField] Scrollbar slider;

    private void Start()
    {
        rightX.action.Enable();
        rightX.action.performed += GrabButton;
        rightX.action.canceled += GrabRelease;

        lr.positionCount = 2;
        aimDirection = Vector3.forward;
        lr.SetPosition(1, aimDirection * 20);
        //rightTrigger.action.performed += ResetButton;
    }

    private void GrabButton(InputAction.CallbackContext context) {
        PressButton();

        lr.positionCount = 2;
        lr.SetPosition(1, aimDirection*20);
    }
    private void GrabRelease(InputAction.CallbackContext context) {
        sliderDragging = false;
    }
    private void ResetButton(InputAction.CallbackContext context) {
        dollhouse.transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        DragSlider();
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
}
