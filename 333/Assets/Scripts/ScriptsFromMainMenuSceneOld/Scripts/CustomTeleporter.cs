using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomTeleporter : MonoBehaviour
{
    [SerializeField] InputActionReference teleportButton; // 
    [SerializeField] GameObject leftController;    // reference to left controller gameObject
    [SerializeField] LineRenderer lr;		// reference to lineRenderer (line that comes out of the end of the controller)
    GameObject ti;				// reference to the flat cylinder that indicates where the user will teleport
    GameObject tiPrefab; 			// prefab for the flat cylinder
    List<GameObject> FadedObjects;		// list of objects made transparent

    Wand wand;	// reference to the wand

    private float maxTeleportDistance = 20f;		// definition for the maximum distance that can be teleported
    private float maxNormalAngle = 45f;		// the maximum angle before a surface is considered a wall and not teleportable

    private void Awake()
    {
        teleportButton.action.performed += EnableTeleport;

    }
    bool AllowTeleport = false;
    void EnableTeleport(InputAction.CallbackContext context) 
    { 
        AllowTeleport= true;
        Debug.Log("anything");
    }

    void Start()
    {
        // define variables on start
        tiPrefab = Resources.Load<GameObject>("Prefabs/TeleportIndicator");
        ti = Instantiate(tiPrefab, Vector3.zero, Quaternion.identity);
        wand = FindObjectOfType<Wand>();
        FadedObjects = new List<GameObject>();
    }

    void Update()
    {
        HandleTeleporter();
    }

    private void HandleTeleporter()
    {
        // reset faded objects to normal materials
        foreach (GameObject g in FadedObjects)
        {
            g.GetComponent<HouseObject>().ResetMyMaterials();
            g.GetComponent<MeshCollider>().enabled = true;

        }
        FadedObjects.Clear();

        Teleport();

        // fade hovered objects that have been wanded to a transparent green
        foreach (GameObject g in FadedObjects)
        {
            wand.SetFaded(g);
            g.GetComponent<MeshCollider>().enabled = false;
        }
    }

    private void Teleport()
    {
        // disable teleport indicator by default
        ti.SetActive(false);


        RaycastHit[] hits;
        // collect list of objects that the teleporter is pointing at as hits
        hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.TransformDirection(Vector3.forward), maxTeleportDistance);
        if (hits.Length > 0) // layerMask
        {
            // sort the list from closest to farthest
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.tag == "Wanded")
                {
                    // if an object has been wanded, it can be passed through
                    FadedObjects.Add(hits[i].transform.gameObject);
                    continue;
                }
                if (Vector3.Angle(Vector3.up, hits[i].normal) < maxNormalAngle)
                {
                    // if it is a valid floor that hasnt been wanded, then put the teleport indicator there
                    ChangeLineRendererColor(Color.green);
                    ti.SetActive(true);
                    ti.gameObject.transform.position = hits[i].point;

                    ////////////

                    if (AllowTeleport)
                    {
                        
                        gameObject.transform.position = hits[i].point;
                    }
                    break;
                }
                else
                {
                    // cannot teleport there
                    ChangeLineRendererColor(Color.red);
                    break;
                }
            }
        }
        else
        {
            // could not find a hit
            ChangeLineRendererColor(Color.red);
        }
        AllowTeleport= false;
    }



    private void ChangeLineRendererColor(Color color)
    {
        lr.startColor = color;
        lr.endColor = color;
    }

}



