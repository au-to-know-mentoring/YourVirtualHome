using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ModelSettings : MonoBehaviour
{
    static string Settings_Location = Application.persistentDataPath + "/" + Application.productName + "Model";
    SettingsScenePlayer settingsScenePlayer; 
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

    void Start()
    {
        settingsScenePlayer = FindObjectOfType<SettingsScenePlayer>();
    }


    /// <summary>
    /// Save dollhouse Positon/Rotation in Settings.txt
    /// </summary>
    /// <param name="path">Model Integer (Model0)</param>
    /// <param name="pos">position(Vector3) to string</param>
    /// <param name="rot">rotation(Quaternion) to string</param>
    public void SaveSettings(string path, string pos, string rot)
    {

        string both = pos + "\n" + rot;// combine position and rotation into 1 string with a line break

        Debug.Log(" File Path: " + path + " Position: " + pos + " Rotation: " + rot);

        File.WriteAllText(Settings_Location + path + "/" + "Settings.txt", both);
    }

    public bool CheckIfSaved()
    {
        return File.Exists(Settings_Location + settingsScenePlayer.ModelVal + "/" + "Settings.txt");
    }

    /// <summary>
    /// Get position from Settings.txt
    /// </summary>
    /// <returns>Vector3 position from saved settings</returns>
    public Vector3 FetchSettingsPosition()
    {
        string[] lines = File.ReadAllLines(Settings_Location + settingsScenePlayer.ModelVal + "/" + "Settings.txt");

        string[] values = lines[0].Trim('(', ')').Split(',');


        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);
        float z = float.Parse(values[2]);

        Vector3 returnPosition = new Vector3(x, y, z);
        return returnPosition;

    }
    
    /// <summary>
    /// Get rotation from Settings.txt
    /// </summary>
    /// <returns>Quaternion rotation from saved settings</returns>
    public Quaternion FetchSettingsRotation()
    {
        string[] lines = File.ReadAllLines(Application.persistentDataPath + "/" + Application.productName + "Model" + settingsScenePlayer.ModelVal + "/" + "Settings.txt");


        string[] values = lines[1].Trim('(', ')').Split(',');

        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);
        float z = float.Parse(values[2]);
        float w = float.Parse(values[3]);



        Quaternion returnRotation = new Quaternion(x, y, z, w);
        return returnRotation;

    }
}
