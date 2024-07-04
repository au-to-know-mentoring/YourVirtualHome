using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Deprecated;
using Unity.VisualScripting;
using UnityEngine;




public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    private void Awake() {
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);
        }
    }
    public void ApplyCollidersToHouse(GameObject parent)
    {
       
        parent.AddComponent<MeshCollider>();
        
        foreach(Transform child in parent.transform)
        {
           
            Debug.Log(child.gameObject.name);
            ApplyCollidersToHouse(child.gameObject);
            
        }
    }
}
