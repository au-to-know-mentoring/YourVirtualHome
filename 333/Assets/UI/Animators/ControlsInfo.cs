using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsInfo : MonoBehaviour
{
    public Animator rhAnimator;
    public Animator lhAnimator;
    public GameObject RHController;
    public GameObject LHController;

    public float ControlZone;

    private bool isEnabled;
    private bool timerStarted;
    private bool timerFinished;
    //private void Update()
    //{
    //    Ray ray = new Ray(headTransform.position, headTransform.forward);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (hit.collider.tag == "RHController")
    //        {
    //            rhAnimator.SetBool("LookingAtController", true);
    //            lhAnimator.SetBool("LookingAtController", false);
    //        }
    //        else if (hit.collider.tag == "LHController")
    //        {
    //            rhAnimator.SetBool("LookingAtController", false);
    //            lhAnimator.SetBool("LookingAtController", true);
    //        }
    //        else
    //        {
    //            rhAnimator.SetBool("LookingAtController", false);
    //            lhAnimator.SetBool("LookingAtController", false);
    //        }
    //    }
    //}

    void Update()
    {
        OpenWhenCloseTogether();
    }


    public void OpenWhenCloseTogether()
    {

        float distance = Vector3.Distance(RHController.transform.position, LHController.transform.position);
        // if (RHController.transform.position == Vector3.zero || LHController.transform.position == Vector3.zero){
        //     rhAnimator.SetBool("LookingAtController", false);
        //     lhAnimator.SetBool("LookingAtController", false);
        // }else {
        if (distance <= ControlZone && !timerStarted)
                {
                    rhAnimator.SetBool("LookingAtController", true);
                    lhAnimator.SetBool("LookingAtController", true);


                    Invoke("StartTimer",3f);
                    timerStarted = true;
                }
                else if (distance > ControlZone && timerFinished)
                {
                    rhAnimator.SetBool("LookingAtController", false);
                    lhAnimator.SetBool("LookingAtController", false);

                    timerFinished = false;
                    timerStarted = false;
                }

    }
 
    


    void StartTimer()
    {
        timerFinished = true;
    }



    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag == "RHController")
    //     {
    //         rhAnimator.SetBool("LookingAtController", true);
    //     }
    //     else if (other.tag == "LHController")
    //     {
    //         lhAnimator.SetBool("LookingAtController", true);
    //     }

    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.tag == "RHController")
    //     {
    //         rhAnimator.SetBool("LookingAtController", false);
    //     }
    //     else if (other.tag == "LHController")
    //     {
    //         lhAnimator.SetBool("LookingAtController", false);
    //     }
    // }
}
