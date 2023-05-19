using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSceneManager : MonoBehaviour
{

    [SerializeField] GameObject dollhouseParent;
    [SerializeField] GameObject player;
    public float perspectiveCompensation = 0.95f;
    Vector3 dummyPosition = new Vector3(0,0,0);

    DollhouseData dd;

    // settings buttons
    [SerializeField] Scrollbar scaleSlider;

    void Start()
    {
        SetupHouseDummy();
    }

    // Update is called once per frame
    void Update()
    {
        scaleDollhouse();
    }

    #region use_settings

    public void scaleDollhouse() {
        if (dollhouseParent == null)
            return;
        float sliderVal = mapToNewRange(0, 1, 0.05f, 1.5f, scaleSlider.value);
        Vector3 newScale = new Vector3(sliderVal, sliderVal, sliderVal);
        dollhouseParent.transform.localScale = newScale;

    }


    public float mapToNewRange(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = OldMax - OldMin;
        float NewRange = NewMax - NewMin;
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public void rotateDollhouse(int xyz) {
        if (Mathf.Abs(xyz) == 1) {
            dollhouseParent.transform.rotation *= Quaternion.Euler(90 * Mathf.Sign(xyz),0,0);
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
        dd.houseRotation = dollhouseParent.transform.rotation;
    }

    public void startRotation() {
        
    }
    public void dragRotation() {
    
    }

    #endregion


    #region setup

    void SetupHouseDummy() {
        GameObject dollhousePrefab = FindObjectOfType<DataManager>().GetHouse();
        GameObject dh = Instantiate(dollhousePrefab, dummyPosition, Quaternion.identity);
        dh.transform.parent = dollhouseParent.transform;
        dh.transform.localPosition = new Vector3(0, 0, 0);
        
        // set layer to dummy house layer
        //SetHouseObjectsLayers(dh);

        // position dollhouse in front of camera
        dollhouseParent.transform.position = dummyPosition;

        Vector3 playerpos = new Vector3(0, 0, FindEdgeOfHouse(dh, 0) - 10);
        player.transform.position = playerpos;
    }

    private float FindEdgeOfHouse(GameObject g, float z) {
        // if no MR, go straight to child
        if (g.GetComponent<MeshRenderer>() == null) {
            z = FindEdgeOfHouse(g.transform.GetChild(0).gameObject, z);
        }
        else if (g.GetComponent<MeshRenderer>().bounds.max.z < z) {
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

    void SetHouseObjectsLayers(GameObject parent) {
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
