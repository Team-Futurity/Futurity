using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Starter : MonoBehaviour
{
	private void Start()
	{
		if (PlayerPrefs.HasKey("Chapter1") == false)
		{
			return;
		}

		if (PlayerPrefs.GetInt("Chapter1") != 1)
		{
			return;
		}

		PlayerPrefs.SetInt("Chapter1", 0);
		ChapterMoveController.Instance.OnEnableController();
	}
}
