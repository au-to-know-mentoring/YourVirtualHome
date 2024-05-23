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
    public Material transparentMat;
    private List<Material> matList = new List<Material>();
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
        if(haveTarget && wandActive)
            addToList(target.transform.gameObject);
            DestroyCollider(target);   // remove collider on object hit by wand
    }

    void ActivateWand(InputAction.CallbackContext context) {
        wandActive = !wandActive;
    }

    void Undo(InputAction.CallbackContext context) 
    {
        if (timer >= delay) 
        {   
            

            for (var x = 0; x < objectList.Count;  x++) {
                objectList[x].GetComponent<MeshRenderer>().material = matList[x];
                objectList[x].GetComponent<MeshCollider>().enabled = true;
            }
            // objectList.Remove(objectList[objectList.Count - 1]);
            // matList.Remove(matList[matList.Count - 1]);

            Debug.Log("objList: " + objectList.Count);
            Debug.Log("matList: " + matList.Count);

            objectList.Clear();
            matList.Clear();

            timer = 0f;
        }
    }

    void addToList(GameObject objectHit)
    {
        
        Debug.Log("addToList");
        Debug.Log("object: " + objectHit.name);
        objectList.Add(objectHit);


        Material material = objectHit.GetComponent<MeshRenderer>().material;
        matList.Add(material);
    
    }

    void Update()
    {
        timer += Time.deltaTime;
        PointWand();
        
    }

    private void PointWand()
    {
        if (!wandActive) {
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
                

                hit.transform.gameObject.tag = "Wanded";
                Debug.Log("Wanded");
                Debug.Log(hit.transform.gameObject.name);
                // MakeObjectTransparent(hit.transform.gameObject);
                // SetFaded(hit.transform.gameObject);
                // addToList(hit.collider.transform.gameObject);
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = transparentMat;
                hit.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                Debug.Log("Collider Destroyed");
            }
    }
    // private void MakeObjectTransparent(GameObject houseObject)
    // {
    //     // get all materials applied to the mesh of a gameobject, and set them to transparency
    //     MeshRenderer mr = houseObject.GetComponent<MeshRenderer>();
    //     Material[] newMaterials = new Material[mr.materials.Length];
    //     for (int i = 0; i < mr.materials.Length; i++)
    //     {
    //         Material m = houseObject.GetComponent<MeshRenderer>().materials[i];
    //         Color c = m.color;
    //         c.a = 125;
    //         m.color = c;
    //         m.SetOverrideTag("RenderType", "Transparent");
    //         m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //         m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //         m.SetInt("_ZWrite", 0);
    //         m.DisableKeyword("_ALPHATEST_ON");
    //         m.EnableKeyword("_ALPHABLEND_ON");
    //         m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //         m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    //         newMaterials[i] = m;

    //     }
    //     mr.materials = newMaterials;

    // }

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
        for (int i = 0; i < mr.materials.Length; i++)
        {
            newMaterials[i] = TransparentDefault;
        }
        mr.materials = newMaterials;
    }

   
    #endregion

    // define reference to house�s gameObject
    public void setHouse(GameObject h)
    {
        house = h;
    }
    /*
    // rotate the house gameObject through the list of rotations
    private void rotateHouse()
    {
        if (house == null)
            return;

        if (Input.GetButtonDown("XRI_Right_SecondaryButton"))
        {
            house.transform.Rotate(Rotations[currentRotation]);
            currentRotation++;
            if (currentRotation >= Rotations.Count)
            {
                currentRotation = 0;
            }
        }
    }
    void createRotationList()
    {
        //  create and add each of 6 rotations to the list
        Rotations = new List<Vector3>();
        Rotations.Add(new Vector3(90, 0, 0));
        Rotations.Add(new Vector3(180, 0, 0));
        Rotations.Add(new Vector3(270, 0, 0));
        Rotations.Add(new Vector3(0, 90, 0));
        Rotations.Add(new Vector3(0, 180, 0));
        Rotations.Add(new Vector3(0, 270, 0));
        Rotations.Add(new Vector3(0, 0, 90));
        Rotations.Add(new Vector3(0, 0, 180));
        Rotations.Add(new Vector3(0, 0, 270));
        Rotations.Add(new Vector3(0, 0, 0));
    }
    */
}



