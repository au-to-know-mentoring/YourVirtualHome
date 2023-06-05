using System.Collections;
using System.IO;
using UnityEngine;
using System.Text;
using Dummiesman;
using UnityEngine.Networking;
using System.Net;
using System;
using System.IO.Compression;
using System.ComponentModel;

public class DownloadHandler : MonoBehaviour
{
    // event for completion of download
    public event System.ComponentModel.AsyncCompletedEventHandler? DownloadFileCompleted;
    // path to zip
    string path = "";

    void Start()
    {
        DownloadFile();
    }

    void DownloadFile()
    {
        WebClient client = new WebClient();

        path = Application.persistentDataPath + "/" + "ArchicadObjSize.zip";
        // links function  to event
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        Uri uri = new Uri("https://github.com/ATK-mentoring/fbx-examples/raw/main/ArchicadObjSize.zip");
        // call download function 
        client.DownloadFileAsync(uri, path);


    }

    void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
    {
        // extract to assets folder
        ZipFile.ExtractToDirectory(path, Application.persistentDataPath);
        var loadedObject = new OBJLoader().Load(Application.persistentDataPath + "/TEST 8/" + "Vacation Home.obj");
        // load object into world 
        loadedObject.gameObject.transform.Rotate(-90f, 0f, 0f, Space.World);
        // apply collision 
        WorldManager.ApplyCollidersToHouse(loadedObject);

        // code for modelview scene
        // give reference of house to wand
        FindObjectOfType<Wand>().setHouse(loadedObject);
        // spawns player near house
        positionPlayer(loadedObject);
        
    }

    void positionPlayer(GameObject house)
    {
        GameObject player = FindObjectOfType<Wand>().gameObject;
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != house)
            {
                player.transform.position = house.transform.position;
            }
        }
    }


    private void OnApplicationQuit()
    {
        DeleteAllFiles();
    }

    public void DeleteAllFiles() {
        ClearFiles("Vacation Home.obj");
        ClearFiles("TEST 8");
        ClearFiles("ArchicadObjSize.zip");
    }
    public void ClearFiles(string path)
    {
        string target = Application.persistentDataPath + "/" + path;
        if (path.Substring(path.Length - 4, 1) == ".")
        {
            File.Delete(target);
        }
        else
        {
            Directory.Delete(target, true);
        }
    }
}