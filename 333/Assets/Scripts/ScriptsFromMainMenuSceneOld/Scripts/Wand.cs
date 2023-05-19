using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    List<Vector3> Rotations;   // list of rotations for the house
    int currentRotation = 0;   // current rotation index in list
    GameObject house = null;   // reference to house game object

    [SerializeField] LineRenderer lr;   // reference to lineRenderer that projects line out of wand

    [SerializeField] GameObject rightController; // reference to right controller

    private float maxWandDistance = 1.6f;   // Maximum interaction distance for wand

    void Start()
    {
        createRotationList();
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

    void Update()
    {
        UseWand();
        rotateHouse();
    }

    private void UseWand()
    {
        RaycastHit hit;
        // fire raycast out of wand
        if (Physics.Raycast(rightController.transform.position, rightController.transform.TransformDirection(Vector3.forward), out hit, maxWandDistance))
        {
            // Debug.DrawRay(rightController.transform.position, rightController.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Did Hit");
            // Debug.DrawRay(hit.point, hit.normal  * 1000, Color.yellow);

            // define colours of line renderer gradient 
            lr.startColor = Color.blue;
            lr.endColor = Color.green;
            DestroyCollider(hit);   // remove collider on object hit by wand

        }
        else
        {
            // Debug.DrawRay(rightController.transform.position, rightController.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            // Debug.Log("Did not Hit");

            // define colours of line renderer gradient 
            lr.startColor = Color.blue;
            lr.endColor = Color.red;
        }
    }

    private void DestroyCollider(RaycastHit hit)
    {
        // when using the right trigger... 
        if (Input.GetButtonDown("XRI_Right_TriggerButton"))
        {

            if (hit.transform.gameObject.GetComponent<MeshCollider>() != null)
            {
                hit.transform.gameObject.tag = "Wanded";
                // MakeObjectTransparent(hit.transform.gameObject);
                // SetFaded(hit.transform.gameObject);
                // Destroy(hit.transform.gameObject.GetComponent<MeshCollider>());
                // Debug.Log("Collider Destroyed");
            }
        }
    }

    private void MakeObjectTransparent(GameObject houseObject)
    {
        // get all materials applied to the mesh of a gameobject, and set them to transparency
        MeshRenderer mr = houseObject.GetComponent<MeshRenderer>();
        Material[] newMaterials = new Material[mr.materials.Length];
        for (int i = 0; i < mr.materials.Length; i++)
        {
            Material m = houseObject.GetComponent<MeshRenderer>().materials[i];
            Color c = m.color;
            c.a = 125;
            m.color = c;
            m.SetOverrideTag("RenderType", "Transparent");
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            newMaterials[i] = m;

        }
        mr.materials = newMaterials;

    }

    #region HandleMaterials

    // reference to transparent material
    [SerializeField] Material TransparentDefault;
    // reference to all materials on gameobject
    Material[] oldMaterials;

    public void SetFaded(GameObject houseObject)
    {
        // access the houseObject’s mesh renderer
        MeshRenderer mr = houseObject.GetComponent<MeshRenderer>();
        Material[] newMaterials = new Material[mr.materials.Length];

        // add a houseObject script to record materials history
        if (houseObject.GetComponent<HouseObject>() == null)
        {
            Material[] oldMaterials = new Material[mr.materials.Length];
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

    // define reference to house’s gameObject
    public void setHouse(GameObject h)
    {
        house = h;
    }

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

}



