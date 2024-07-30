using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wand : MonoBehaviour
{
    //List<Vector3> Rotations;   // list of rotations for the house
    //int currentRotation = 0;   // current rotation index in list


    GameObject house = null;   // reference to house game object

    [SerializeField] LineRenderer lr;   // reference to lineRenderer that projects line out of wand

    [SerializeField] GameObject leftController; // reference to right controller
    [SerializeField] InputActionReference wandButton;
    [SerializeField] InputActionReference wandActivateButton;
    [SerializeField] InputActionReference wandUndo;


    bool wandActive = false;
    Vector3 aimDirection = Vector3.forward;
    private float maxWandDistance = 2.5f;   // Maximum interaction distance for wand
    private bool haveTarget = false; //boolean to confirm if wand should be useable and is on-target
    private RaycastHit target; // storing target for wand use
    private List<GameObject> objectList = new List<GameObject>();

    private float timer = 0f;
    private float delay = 0.05f;

    void Start()
    {
        wandUndo.action.Enable();
        wandButton.action.Enable();
        wandActivateButton.action.Enable();
        wandUndo.action.performed += Undo;
        wandButton.action.performed += UseWand;
        wandActivateButton.action.performed += ActivateWand;
        wandActivateButton.action.canceled += ActivateWand;
    }

    void UseWand(InputAction.CallbackContext context)
    {
        if (haveTarget && wandActive)
            addToList(target.transform.gameObject);
        DestroyCollider(target);   // remove collider on object hit by wand
    }

    void ActivateWand(InputAction.CallbackContext context)
    {
        wandActive = !wandActive;
    }

    void Undo(InputAction.CallbackContext context)
    {
        EnableComponents(house);
    }

    /// <summary>
    /// re-enable mesh renderer and collider
    /// </summary>
    /// <param name="parent"></param>
    public void EnableComponents(GameObject parent)
    {

        Debug.Log("ObjectName: " + parent.name);


        if (parent.GetComponent<MeshCollider>() != null && parent.GetComponent<MeshRenderer>() != null)
        {
            parent.GetComponent<MeshCollider>().enabled = true;

            parent.GetComponent<MeshRenderer>().enabled = true;
        }

        Debug.Log(parent.transform.childCount);

        foreach (Transform child in parent.transform)
        {
            Debug.Log("Child ObjectName: " + child.gameObject.name);
            if (child.GetComponent<MeshCollider>().enabled == false){
                EnableComponents(child.gameObject);
            }

        }

    }

    void addToList(GameObject objectHit)
    {

        Debug.Log("addToList");
        Debug.Log("object: " + objectHit.name);
        objectList.Add(objectHit);

    }

    void Update()
    {
        timer += Time.deltaTime;
        PointWand();

    }

    private void PointWand()
    {
        if (!wandActive)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;
        RaycastHit hit;
        // fire raycast out of wand
        if (Physics.Raycast(leftController.transform.position, leftController.transform.TransformDirection(aimDirection), out hit, maxWandDistance))
        {
            // define colours of line renderer gradient 
            ChangeLineRendererColor(Color.green);
            haveTarget = true;
            target = hit;

        }
        else
        {
            // define colours of line renderer gradient 
            ChangeLineRendererColor(Color.red);
            haveTarget = false;
        }
    }

    private void DestroyCollider(RaycastHit hit)
    {

        if (hit.transform.gameObject.GetComponent<MeshCollider>() != null)
        {

            // hit.transform.gameObject.tag = "Wanded";
            Debug.Log(hit.transform.gameObject.name + ": Wanded");

            hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            hit.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
            Debug.Log("Collider Destroyed");
        }
    }

    private void ChangeLineRendererColor(Color color)
    {
        lr.positionCount = 2;
        lr.SetPosition(1, aimDirection * maxWandDistance);
        lr.startColor = Color.blue;
        lr.endColor = color;
    }
    #region HandleMaterials

    // reference to transparent material
    [SerializeField] Material TransparentDefault;
    // reference to all materials on gameobject
    Material[] oldMaterials;

    public void SetFaded(GameObject houseObject)
    {
        // access the houseObject�s mesh renderer
        MeshRenderer mr = houseObject.GetComponent<MeshRenderer>();
        Material[] newMaterials = new Material[mr.materials.Length];

        // add a houseObject script to record materials history
        if (houseObject.GetComponent<HouseObject>() == null)
        {
            oldMaterials = new Material[mr.materials.Length];
            oldMaterials = mr.materials;
            houseObject.AddComponent<HouseObject>();
            houseObject.GetComponent<HouseObject>().SetMyMaterials(oldMaterials);

        }
        // set all materials to new transparent material

    }


    #endregion

    // define reference to house�s gameObject
    public void setHouse(GameObject h)
    {
        house = h;
    }
   
}