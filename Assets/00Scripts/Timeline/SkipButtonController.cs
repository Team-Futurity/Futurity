using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipButtonController : MonoBehaviour
{
	[Header("SkipButtonInfo")]
	[SerializeField, ReadOnly(false)] private AutoSkipButton autoSkipButton;
	[SerializeField, ReadOnly(false)] private bool isEnable = false; 
	
	private void OnEnable()
	{
		if (autoSkipButton != null)
		{
			return;
		}

		if (GetRootObject().TryGetComponent(out autoSkipButton) == false)
		{
			return;
		}

		isEnable = true;
	}

	private void Update()
	{
		if (isEnable == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			autoSkipButton.SkipCutScene();
			isEnable = false;
		}
	}

	private void OnDisable()
	{
		isEnable = false;
	}

	private GameObject GetRootObject()
	{
		Transform curTf = transform;

		while (curTf.parent != null)
		{
			curTf = curTf.parent;
		}

		return curTf.gameObject;
	}
}
