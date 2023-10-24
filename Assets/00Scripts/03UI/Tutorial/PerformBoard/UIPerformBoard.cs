using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoard : MonoBehaviour
{
	// TargetCondition
	[SerializeField]
	private PlayerInputEnum targetCondition;

	// Enter : Player Input Enum
	// Check : Key Checked

	public bool EnterPlayerEventType(PlayerInputEnum type)
	{
		return CheckCondition(type);
	}

	public void Active(bool isOn)
	{
		gameObject.SetActive(isOn);
	}

	private bool CheckCondition(PlayerInputEnum type)
	{
		if(targetCondition != type)
		{
			return false;
		}

		return true;
	}
}
