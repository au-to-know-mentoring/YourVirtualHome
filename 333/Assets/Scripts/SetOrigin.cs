using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrigin : MonoBehaviour
{
    public GameObject originPoint;
    public GameObject house;
	//private void Awake()
	//{
	//	Instantiate(originPoint);
	//}

	// Update is called once per frame
	void Update()
    {
		//testMove();

		if (Input.GetKeyDown(KeyCode.N))
        {
            PlaceOrigin();

		}
    }

	void PlaceOrigin()
	{
		house.transform.SetParent(originPoint.transform, false);
	}

	//void testMove()
	//{
	//	if (Input.GetKeyDown(KeyCode.L))
	//	{
	//		house.transform.position = new Vector3(10, 10, 10);
	//	}else if (Input.GetKeyDown(KeyCode.K))
	//	{
	//		originPoint.transform.position = new Vector3(20, 20, 20);
	//	}
	//}
}
