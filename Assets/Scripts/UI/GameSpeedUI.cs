using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedUI : MonoBehaviour
{
	public TMP_InputField field;

	public void SetTimeScale(string text)
	{
		Time.timeScale = float.Parse(text);
	}

}
