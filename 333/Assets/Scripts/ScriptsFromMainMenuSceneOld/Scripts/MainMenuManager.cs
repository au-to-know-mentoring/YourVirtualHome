using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public ListItem selectedValue;

    public void SetSelection(ListItem value)
    {
        selectedValue= value;
    }

    public ListItem GetSelection()
    {
        return selectedValue;
    }

    public void DeleteSelection()
    {
        if (selectedValue == null)
        {
            return;
        }
        // deletion from server functionality to be added
        Destroy(selectedValue.gameObject);
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
