using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    // Adjust via the Inspector
    public int maxLines = 8;
    private int errorCount = 0;
    private Queue<string> queue = new Queue<string>();
    private string currentText = "";

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        errorCount++;
        // Delete oldest message
        if (queue.Count >= maxLines) queue.Dequeue();

        queue.Enqueue("Error #" + errorCount + ": " + logString);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        currentText = builder.ToString();
        AdjustCanvas();
    }

    [SerializeField] TextMeshProUGUI textElement;

    private void AdjustCanvas() {
        textElement.text = currentText;
    }

    /*void OnGUI()
    {
        GUI.Label(
           new Rect(
               5,                   // x, left offset
               Screen.height - 150, // y, bottom offset
               300f,                // width
               150f                 // height
           ),
           currentText,             // the display text
           GUI.skin.textArea        // use a multi-line text area
        );
    }
    */
}