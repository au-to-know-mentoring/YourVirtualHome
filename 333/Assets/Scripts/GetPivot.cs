using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPivot : MonoBehaviour
{

    GameObject ModelCollector;
  

    private void Awake() 
    {
        ModelCollector = Instantiate(new GameObject());
        CalcCentre();
        
    }

    void CalcCentre()
    {
        
        gameObject.transform.SetParent(ModelCollector.transform);
        // gameObject.transform.SetParent(ModelCollector.transform);

        Vector3 sumOfCentrePoints = Vector3.zero;
        int countOfPoints = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 centre = MoveMesh(i);
            if (centre != Vector3.zero)
            {
                sumOfCentrePoints += centre;
                countOfPoints++;
            }
        }
        
        sumOfCentrePoints /= countOfPoints; //final centre of whole mesh
        transform.position -= sumOfCentrePoints;
        // Move ModelCollector into position
        ModelCollector.transform.SetParent(FindObjectOfType<DownloadHandler>().ModelHolderParent.transform);
        ModelCollector.transform.localPosition = Vector3.zero;

        // reparent to DollHouse
        gameObject.transform.SetParent(ModelCollector.transform.parent);

        // Destroy ModelCollector
        Destroy(ModelCollector);
        

    }

Vector3 MoveMesh(int childIndex)
{
	Vector3 centre = transform.GetChild(childIndex).GetComponent<MeshRenderer>() == null ? Vector3.zero :  transform.GetChild(childIndex).GetComponent<MeshRenderer>().bounds.center;
	return centre;
}


void UnParent()
{
    gameObject.transform.SetParent(null);
}
 

  
   
}
