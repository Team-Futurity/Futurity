using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFadeIn : MonoBehaviour
{
	private void Start()
	{
		FadeManager.Instance.FadeOut();
	}
}
