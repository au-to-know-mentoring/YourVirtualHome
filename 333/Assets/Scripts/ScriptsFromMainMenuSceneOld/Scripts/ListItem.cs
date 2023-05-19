using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItem : MonoBehaviour
{
    int house = -1;




    public void SetValue(int value) 
    {
        house = value;

    }
    public int GetValue()
    {
        return  house;

    }
    public void setselection()
    {
        FindObjectOfType<MainMenuManager>().SetSelection(this);   
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
