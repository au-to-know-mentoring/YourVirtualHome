using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HouseObject : MonoBehaviour
{
    Material[] myMaterials;


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
