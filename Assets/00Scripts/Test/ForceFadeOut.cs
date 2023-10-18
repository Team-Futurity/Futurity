using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFadeOut : MonoBehaviour
{
	private void Start()
	{
		FadeManager.Instance.ForceFadeOut();
	}
}
