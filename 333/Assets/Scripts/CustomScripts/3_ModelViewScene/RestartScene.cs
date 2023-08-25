using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    float timer;
    float holdDur = 5f;
    // Update is called once per frame
    void Update()
    {
        // Check for input to restart the scene (you can change this condition as needed)
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            timer = Time.time;
        }
        //else if (Input.GetKey(KeyCode.L))
            else if (OVRInput.Get(OVRInput.Button.Two))
        {
            if (Time.time - timer > holdDur)
            {
                //by making it positive inf, we won't subsequently run this code by accident,
                //since X - +inf = -inf, which is always less than holdDur
                timer = float.PositiveInfinity;


                //perform your action
               // Debug.Log("Held For 3 Seconds");
                RestartCurrentScene();
            }
        }
        else
        {
            timer = float.PositiveInfinity;
        }
    }

    // Function to restart the current scene
    public void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
}

