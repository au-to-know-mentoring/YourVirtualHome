using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // set up singleton
    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [SerializeField] GameObject housePrefab; // stored base version of downloaded house
    GameObject spawnPosition;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(housePrefab.name + housePrefab.transform.localScale);
    }

    public void SetHouse(GameObject h)
    {
        housePrefab = h;
    }
    public void SetSpawnPosition(GameObject sp) {
        spawnPosition = sp;
    }
    public GameObject GetSpawnPosition() {
        return spawnPosition;
    }
    public GameObject GetHouse() {
        return housePrefab;
    }
}
