using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSceneManager : MonoBehaviour
{
	[SerializeField] GameObject originPrefab;

	[SerializeField] GameObject dollhouseParent;
    [SerializeField] SettingsScenePlayer playerScript;
    public float perspectiveCompensation = 0.95f;
    Vector3 dummyPosition = new Vector3(0, 0, 0);


    void Start()
    {
        // SetupHouseDummy();
    }

    // Update is called once per frame
    void Update()
    {
        //   scaleDollhouse();
    }

    #region use_settings

    public float mapToNewRange(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = OldMax - OldMin;
        float NewRange = NewMax - NewMin;
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public void rotateDollhouse(int xyz)
    {
        if (Mathf.Abs(xyz) == 1)
        {
            dollhouseParent.transform.rotation *= Quaternion.Euler(90 * Mathf.Sign(xyz), 0, 0);
        }
        else if (Mathf.Abs(xyz) == 2)
        {
            dollhouseParent.transform.rotation *= Quaternion.Euler(0, 90 * Mathf.Sign(xyz), 0);
        }
        else if (Mathf.Abs(xyz) == 3)
        {
            dollhouseParent.transform.rotation *= Quaternion.Euler(0, 0, 90 * Mathf.Sign(xyz));
        }
        // record new rotation
        //dd.houseRotation = dollhouseParent.transform.rotation;
    }

    public void moveDollhouse(int xyz)
    {
        if (Mathf.Abs(xyz) == 1)
        {
            dollhouseParent.transform.position += new Vector3(0.25f * Mathf.Sign(xyz), 0, 0);
        }
        else if (Mathf.Abs(xyz) == 2)
        {
            dollhouseParent.transform.position -= new Vector3(0.25f * Mathf.Sign(xyz), 0, 0);
        }
        else if (Mathf.Abs(xyz) == 3)
        {
            dollhouseParent.transform.position += new Vector3(0, 0.25f * Mathf.Sign(xyz), 0);
        }else if (Mathf.Abs(xyz) == 4)
        {
            dollhouseParent.transform.position -= new Vector3(0, 0.25f * Mathf.Sign(xyz), 0);
        }
        else if (Mathf.Abs(xyz) == 5)
        {
            dollhouseParent.transform.position += new Vector3(0, 0, 0.25f * Mathf.Sign(xyz));
        }else if (Mathf.Abs(xyz) == 6)
        {
            dollhouseParent.transform.position -= new Vector3(0, 0, 0.25f * Mathf.Sign(xyz));
        }
        //  if (Mathf.Abs(xyz) == 1)
        // {
        //     dollhouseParent.transform.position *= Quaternion.Euler(90 * Mathf.Sign(xyz), 0, 0);
        // }
        // else if (Mathf.Abs(xyz) == 2)
        // {
        //     dollhouseParent.transform.position *= Quaternion.Euler(0, 90 * Mathf.Sign(xyz), 0);
        // }
        // else if (Mathf.Abs(xyz) == 3)
        // {
        //     dollhouseParent.transform.position *= Quaternion.Euler(0, 0, 90 * Mathf.Sign(xyz));
        // }
        // record new rotation
        //dd.houseRotation = dollhouseParent.transform.rotation;
    }

    public void setNewOrigin()
    {
        Instantiate(originPrefab, dollhouseParent.transform);
    }

    public void EnablePlaceSpawnerButton()
    {
        playerScript.EnableSpawnerPlacement();
    }
    #endregion


    #region setup

    public void SetupHouseDummy()
    {

        var dh = FindObjectOfType<DataManager>().GetHouse();
        dh.transform.SetParent(dollhouseParent.transform);

    }

    private float FindEdgeOfHouse(GameObject g, float z)
    {
        // if no MR, go straight to child
        if (g.GetComponent<MeshRenderer>() == null)
        {
            z = FindEdgeOfHouse(g.transform.GetChild(0).gameObject, z);
        }
        else if (g.GetComponent<MeshRenderer>().bounds.max.z < z)
        {
            z = g.GetComponent<MeshRenderer>().bounds.max.z;
            // only check children if headed in the positive X direction to optimise speed
            if (g.transform.childCount > 0)
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    float f = FindEdgeOfHouse(g.transform.GetChild(i).gameObject, z);
                    if (f < z)
                    {
                        z = f;
                    }
                }
            }
        }

        return z;
    }

    void SetHouseObjectsLayers(GameObject parent)
    {
        parent.layer = 3;

        foreach (Transform child in parent.transform)
        {
            SetHouseObjectsLayers(child.gameObject);
        }
    }

    Bounds getBounds(GameObject objeto)
    {
        Bounds bounds;
        Renderer childRender;
        bounds = getRenderBounds(objeto);
        if (bounds.extents.x == 0)
        {
            bounds = new Bounds(objeto.transform.position, Vector3.zero);
            foreach (Transform child in objeto.transform)
            {
                childRender = child.GetComponent<Renderer>();
                if (childRender)
                {
                    bounds.Encapsulate(childRender.bounds);
                }
                else
                {
                    bounds.Encapsulate(getBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }

    Bounds getRenderBounds(GameObject objeto)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer render = objeto.GetComponent<Renderer>();
        if (render != null)
        {
            return render.bounds;
        }
        return bounds;
    }
    #endregion
}
