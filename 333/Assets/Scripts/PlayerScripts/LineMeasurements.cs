using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class LineMeasurements : MonoBehaviour
{
    GameObject point1;
    GameObject point2;
    bool secondPlaced = false;
    public TMP_Text displayText;
    public GameObject displayCanvas;

    public GameObject Controller;

    public LineRenderer lineRenderer;
    [SerializeField] private Material lineMat;

	[SerializeField] public InputActionReference MeasurementButton;


	private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.enabled = true;
        // lineRenderer.material = lineMat;
		MeasurementButton.action.Enable();
        MeasurementButton.action.performed += SetPointFromControllerButton;

	}


    void SetPoint(Vector3 position)
    {
        //place point 1
        if (point1 == null || secondPlaced)
        {
            if (secondPlaced)
            {
                Destroy(point1);
                Destroy(point2);
                //lineRenderer.SetPosition(0, point1.transform.position);
                //lineRenderer.SetPosition(1, point2.transform.position);

                //lineRenderer.enabled = false;
            }

            point1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point1.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            point1.transform.position = position;

            lineRenderer.SetPosition(0, point1.transform.position);
            secondPlaced = false;
        }
        else if (!secondPlaced) //place point 2
        {
            secondPlaced = true;
            point2.transform.position = position;
            lineRenderer.SetPosition(0, point1.transform.position);
            lineRenderer.SetPosition(1, point2.transform.position);
            lineRenderer.enabled = true;
        }
    }
    public void DisplayInformation()
    {
        float magnitude = (point2.transform.position-point1.transform.position).magnitude;
        float halfMagnitude = magnitude * 0.5f;
        Vector3 unitVec = (point2.transform.position + point1.transform.position) * 0.5f;
        //unitVec *= halfMagnitude;

        displayCanvas.transform.position = unitVec;
        displayText.text = Math.Round(((point1.transform.position - point2.transform.position).magnitude), 2) + "m";
        displayCanvas.transform.forward = Camera.main.transform.forward;
        displayCanvas.transform.position -= displayCanvas.transform.forward * 0.25f;


	}

    public void SetPointFromControllerButton(InputAction.CallbackContext context)
    {
		RaycastHit hit;

		if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit))
		{
			SetPoint(hit.point);
            
		}
	}

    private void Update()
    {
		//if (point1 != null && point2 != null)
		//{
		//	Debug.Log("pos: " + Math.Round(((point1.transform.position - point2.transform.position).magnitude),2) + "m");
		//}
		//if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit hit;
//
        //    if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit))
        //    {
        //        SetPoint(hit.point);
         //   }
       // }

        if (point1 != null && !secondPlaced)
        {
			

			if (point2 == null)
            {
                point2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                point2.GetComponent<Collider>().enabled = false;
				point2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

			}

			RaycastHit hit;

            if (Physics.Raycast(Controller.transform.position,Controller.transform.forward, out hit))
            {
                point2.transform.position = hit.point;
                lineRenderer.SetPosition(1, point2.transform.position);

				//SetPoint(hit.point);
			}
			DisplayInformation();
		}
    }
}
