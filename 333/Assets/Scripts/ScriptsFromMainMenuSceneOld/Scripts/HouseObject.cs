using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HouseObject : MonoBehaviour
{
    Material[] myMaterials;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMyMaterials(Material[] m)
    {
        myMaterials = m;
    }
    public void ResetMyMaterials()
    {
        MeshRenderer m = gameObject.GetComponent<MeshRenderer>();
        m.materials = myMaterials;

    }


}
