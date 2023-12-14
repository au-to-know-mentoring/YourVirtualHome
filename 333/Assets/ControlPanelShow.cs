using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelShow : MonoBehaviour
{
	public GameObject ControlPanel;
	private void Start()
	{
		ControlPanel.active = false;
	}
	public void showControlPanel()
	{
		if (ControlPanel.active != true)
		{
			ControlPanel.active = true;
		}
		else { ControlPanel.active = false; }
	}
}
