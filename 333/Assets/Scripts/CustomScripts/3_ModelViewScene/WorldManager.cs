using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public static class WorldManager : object
{
   
    public static void ApplyCollidersToHouse(GameObject parent)
    {
        parent.AddComponent<MeshCollider>();
       
        foreach(Transform child in parent.transform)
        {
            ApplyCollidersToHouse(child.gameObject);
        }
    }
}
