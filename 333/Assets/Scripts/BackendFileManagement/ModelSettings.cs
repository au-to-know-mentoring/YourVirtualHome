using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModelSettings : MonoBehaviour
{

    public static ModelSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SaveSettings(string path, string pos, string rot)
    {

        string both = pos + "\n" + rot;// combine position and rotation into 1 string with a line break

        Debug.Log(" File Path: " + path + " Position: " + pos + " Rotation: " + rot);

        File.WriteAllText(Application.persistentDataPath + "/" + Application.productName + "Model" + path + "/" + "Settings.txt", both);
    }

    public string FetchSettingsPosition()
    {
        string[] lines = File.ReadAllLines(Application.persistentDataPath + "/" + Application.productName + "Model" + FindObjectOfType<SettingsScenePlayer>().modelVal + "/" + "Settings.txt");
        Debug.Log(lines[0]);
        Debug.Log(lines[1]);

        return lines[0];

    }

    public string FetchSettingsRotation()
    {
        string[] lines = File.ReadAllLines(Application.persistentDataPath + "/" + Application.productName + "Model" + FindObjectOfType<SettingsScenePlayer>().modelVal + "/" + "Settings.txt");
        Debug.Log(lines[0]);
        Debug.Log(lines[1]);

        return lines[1];

    }
}
