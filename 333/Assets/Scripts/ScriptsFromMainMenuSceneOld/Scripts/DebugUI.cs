using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugUI : MonoBehaviour
{
    [SerializeField] GameObject debugTextBox;
    private int logCount = 0;
    private float scrollOffSet = 0f;
    // Start is called before the first frame update
    void Start()
    {
        LogToCanvas("Log Log Log");
        LogToCanvas("L L L");
        LogToCanvas("O O O");
        LogToCanvas("G G G");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LogToCanvas(string log)
    {
        debugTextBox.SetActive(true);
        logCount++;
        float textSize = debugTextBox.GetComponent<TextMeshProUGUI>().fontSize;
        float xValue = debugTextBox.GetComponent<RectTransform>().sizeDelta.x;
        debugTextBox.GetComponent<RectTransform>().sizeDelta = new Vector2(xValue, textSize * logCount * 1.1f);
        debugTextBox.GetComponent<TextMeshProUGUI>().text += log + "\n";
        debugTextBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, (textSize / 2) * logCount + scrollOffSet);

        
    }
}
