using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CustomTeleporter : MonoBehaviour
{
    // record input of secondary buttons for scene reset
    [SerializeField] InputActionReference leftSecondary;
    [SerializeField] InputActionReference rightSecondary;
    private bool leftSecondaryPressed = false;
    private bool rightSecondaryPressed = false;
    [SerializeField] GameObject canvas;     // canvas reference for scene reset
    private bool readyToReset = false; // bool to determine if the user is hovering the reset canvas

    [SerializeField] DownloadHandler dh;

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
        // enable controls
        teleportButton.action.Enable();
        teleportButton.action.performed += EnableTeleport;

        leftSecondary.action.Enable();
        rightSecondary.action.Enable();
        leftSecondary.action.performed += LeftSecondary;
        leftSecondary.action.canceled += LeftSecondaryOff;
        rightSecondary.action.performed += RightSecondary;
        rightSecondary.action.canceled += RightSecondaryOff;

    }
    bool AllowTeleport = false;
    void EnableTeleport(InputAction.CallbackContext context) 
    { 
        AllowTeleport= true;
    }
    void LeftSecondary(InputAction.CallbackContext context) {
        leftSecondaryPressed = true;
    }
    void RightSecondary(InputAction.CallbackContext context)
    {
        rightSecondaryPressed = true;
    }
    void LeftSecondaryOff(InputAction.CallbackContext context)
    {
        leftSecondaryPressed = false;
    }
    void RightSecondaryOff(InputAction.CallbackContext context)
    {
        rightSecondaryPressed = false;
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
        HandleSceneReset();
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
        hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.TransformDirection(Vector3.down), maxTeleportDistance);
        if (hits.Length > 0) // layerMask
        {
            // sort the list from closest to farthest
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.tag == "SceneReset") {
                    readyToReset = true;
                    break;
                }
                else
                {
                    readyToReset = false;
                }
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
        lr.positionCount = 2;
        lr.SetPosition(1, new Vector3(0, -maxTeleportDistance, 0));
        lr.startColor = color;
        lr.endColor = color;
    }

    private void HandleSceneReset() {
        if (!leftSecondaryPressed || !rightSecondaryPressed) {
            if (readyToReset) {
                // clear files
                dh.DeleteAllFiles();
                // reload scene on release
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            // otherwise hide canvas
            canvas.SetActive(false);
            return;
        }

        // if both secondaries are pressed, enable canvas
        canvas.SetActive(true);
    }

}



