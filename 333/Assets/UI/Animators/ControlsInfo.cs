using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsInfo : MonoBehaviour
{
    public Animator rhAnimator;
    public Animator lhAnimator;
    public Transform headTransform;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RHController")
        {
            rhAnimator.SetBool("LookingAtController", true);
        }
        else if (other.tag == "LHController")
        {
            lhAnimator.SetBool("LookingAtController", true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RHController")
        {
            rhAnimator.SetBool("LookingAtController", false);
        }
        else if (other.tag == "LHController")
        {
            lhAnimator.SetBool("LookingAtController", false);
        }
    }
}
