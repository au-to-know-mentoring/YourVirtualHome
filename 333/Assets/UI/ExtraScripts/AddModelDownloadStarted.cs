using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddModelDownloadStarted : MonoBehaviour
{
    [SerializeField] private RectTransform downloadButton;
    private float downloadBaseY;
    public float downloadTopY;

    [SerializeField] private Slider downloadProgressSlider;
    [SerializeField] private Image downloadingBar;
    [SerializeField] private Image downloadingBG;
    [SerializeField] private TMP_Text downloadingText;
    [SerializeField] private TMP_Text downloadPercentage;

    private bool isDownloading;
    private void Awake()
    {
        downloadBaseY = downloadButton.position.y;
        
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

    public IEnumerator MoveButtonUp()
    {
        if (downloadButton.localPosition.y < downloadTopY)
        {
            downloadButton.position += new Vector3(0, 0.01f, 0f);
        }
        else
        {
			
			
            // isDownloading = true;
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
	public IEnumerator DownloadSliderProgress()
	{
		if (FindObjectOfType<DownloadHandler>().ProgressVar.ProgressPercentage <= 100)
		{
            downloadProgressSlider.value = FindObjectOfType<DownloadHandler>().ProgressVar.ProgressPercentage;
            downloadPercentage.text = downloadProgressSlider.value.ToString() + "%";
		}
		else
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.01f);
		StartCoroutine(DownloadSliderProgress());
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
	private void Update()
	{

        // if (FindObjectOfType<DownloadHandler>().ProgressVar.ProgressPercentage <= 100)
		// {
        //     if (isDownloading){
        //         downloadProgressSlider.value = FindObjectOfType<DownloadHandler>().ProgressVar.ProgressPercentage;
        //     downloadPercentage.text = downloadProgressSlider.value.ToString() + "%";
        //     }
            
		// }else {
        //     isDownloading = false;
        // }


	}
}
