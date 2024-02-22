using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddModelDownloadStarted : MonoBehaviour
{
    [SerializeField] private RectTransform downloadButton;
    private float downloadBaseY;
    public float downloadTopY;


    [SerializeField] private Image downloadingBar;
    [SerializeField] private Image downloadingBG;
    [SerializeField] private TMP_Text downloadingText;
    [SerializeField] private TMP_Text downloadPercentage;

    private void Awake()
    {
        downloadBaseY = downloadButton.position.y;
        //StartCoroutine(WaitForLoad());
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Fake Download", GUILayout.MinWidth(60), GUILayout.MinHeight(30))) 
        {
            StartCoroutine(MoveButtonUp());
        }
    }
    public void DownloadStarted()
    {
        StartCoroutine(MoveButtonUp());
    }

    private IEnumerator MoveButtonUp()
    {
        if (downloadButton.localPosition.y < downloadTopY)
        {
            downloadButton.position += new Vector3(0, 0.01f, 0f);
        }
        else
        {
            StartCoroutine(FadeDownloadIn());
            yield return null;
        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(MoveButtonUp());
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(MoveButtonUp());
    }

    private IEnumerator FadeDownloadIn()
    {
        if (downloadingBar.color.a < 1f)
        {
            downloadingBar.color += new Color(0, 0, 0, 0.01f);
            downloadingBG.color += new Color(0, 0, 0, 0.01f);
            downloadingText.color += new Color(0, 0, 0, 0.01f);
            downloadPercentage.color += new Color(0, 0, 0, 0.01f);
        }
        else 
        { 
            yield return null; 
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(FadeDownloadIn());

    }
}
