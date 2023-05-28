using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
	public void TextDebugLog(string text)
	{
		FDebug.Log(text);
	}
}
