using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefultWindowController : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro mainText;
	[SerializeField]
	private TextMeshPro rightButtonText;
	[SerializeField]
	private TextMeshPro leftButtonText;


	public void SetText(string mainTextInput, string rightButtonTextInput, string leftButtonTextInput)
	{
		mainText.text = mainTextInput;
		rightButtonText.text = rightButtonTextInput;
		leftButtonText.text = leftButtonTextInput;
	}
}
