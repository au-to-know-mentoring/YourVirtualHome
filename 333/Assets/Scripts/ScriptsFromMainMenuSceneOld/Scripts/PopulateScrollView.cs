using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateScrollView : MonoBehaviour
{
    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private int m_ItemsToGenerate;

    void Start()
    {
        for (int i = 0; i < m_ItemsToGenerate; i++)
        {
            var item_go = Instantiate(m_ItemPrefab);
            // do something with the instantiated item -- for instance
            item_go.GetComponentInChildren<Text>().text = "Persephone Jaxon" + i;
            //item_go.GetComponent<Image>().color = i % 2 == 0 ? Color.yellow : Color.cyan;
            //parent the item to the content container
            item_go.transform.SetParent(m_ContentContainer);
            //reset the item's scale -- this can get munged with UI prefabs
            item_go.transform.localScale = Vector2.one;

            item_go.GetComponent<ListItem>().SetValue(i);
        }
    }

    /*  static void main(string[] args)
     {
         List<string> ListOfCustomers = new List<string>();
         {
             "Persephone Jaxon",
             "Clementine klavsoki",
             "Chrysanthemum McLoad",
             "Magdalena robinson"
         };
     }*/
}