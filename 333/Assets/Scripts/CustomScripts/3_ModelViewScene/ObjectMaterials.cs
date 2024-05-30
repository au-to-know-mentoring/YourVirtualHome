using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterials : MonoBehaviour
{
    public Material[] materials;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    private void Awake() {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
       
        materials = new Material[meshRenderer.materials.Length];
        materials = meshRenderer.materials;
    }

    public void ResetMaterials(){
        meshRenderer.materials = materials;
    }

   
}
