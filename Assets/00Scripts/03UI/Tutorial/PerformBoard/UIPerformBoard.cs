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

	private bool[] moveType = new bool[4];

	[HideInInspector]
	public UnityAction onEndedAction; 

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
		if (targetCondition == PlayerInputEnum.Move)
		{
			if (type == PlayerInputEnum.Move_Up)
				moveType[0] = true;
			if (type == PlayerInputEnum.Move_Down)
				moveType[1] = true;
			if (type == PlayerInputEnum.Move_Left)
				moveType[2] = true;
			if (type == PlayerInputEnum.Move_Right)
				moveType[3] = true;

			for (int i = 0; i < 4; ++i)
			{
				if (moveType[i] == false)
				{
					return false;
				}
			}
		}
		else
		{
			if (targetCondition != type)
			{
				return false;
			}
		}

		onEndedAction?.Invoke();
		return true;
	}
}
