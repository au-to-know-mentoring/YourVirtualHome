using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlafSceneHandler : MonoBehaviour
{
    [SerializeField] GameObject house;
    void Start()
    {
        WorldManager.ApplyCollidersToHouse(house);
        FindObjectOfType<Wand>().setHouse(house);
    }
}
