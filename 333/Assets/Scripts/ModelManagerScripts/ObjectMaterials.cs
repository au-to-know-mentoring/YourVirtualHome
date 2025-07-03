using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterials : MonoBehaviour
{
    public Material[] materials;
    Material[] newMaterials;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    private void Awake() {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
       
        materials = new Material[meshRenderer.materials.Length];

        
        materials = meshRenderer.materials;

        
    }

    public void SetFaded(){
        newMaterials = new Material[meshRenderer.materials.Length];

        
         for (int i = 0; i < newMaterials.Length; i++)
        {
            Color transparent = Color.green;
            transparent.a = 0.2f;

            Debug.Log("Mat Name: " +newMaterials[i].name);
            newMaterials[i].color = transparent;

            Debug.Log("setFaded: " + newMaterials[i].name);

        }
        meshRenderer.materials = newMaterials;
    }

    public void ResetMaterials(){

        Debug.Log(gameObject.name + ": RESET MY MATERIALS");

        
        meshRenderer.materials = materials;
       
    }

   
}
