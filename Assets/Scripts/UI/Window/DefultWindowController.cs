using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefultWindowController : MonoBehaviour
{
	[SerializeField]
	private Text mainText;
	[SerializeField]
	private Text rightButtonText;
	[SerializeField]
	private Text leftButtonText;


	public void SetText(string mainTextInput, string rightButtonTextInput, string leftButtonTextInput)
	{
		mainText.text = mainTextInput;
		rightButtonText.text = rightButtonTextInput;
		leftButtonText.text = leftButtonTextInput;
	}
}
