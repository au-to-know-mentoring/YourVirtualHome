using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicScrollView : MonoBehaviour
{
    [SerializeField]
    private Transform ScrollViewContent;

    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private List<GameObject> PrefabList;




    private void Start()
    {
        foreach(GameObject house in PrefabList)
        {
             GameObject newHouse = Instantiate(Prefab, ScrollViewContent);
          

        }
    }






}
