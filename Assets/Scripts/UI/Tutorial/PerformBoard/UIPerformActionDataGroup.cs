using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ActionImageType
{
	NONE = 0,
	
	NORMAL,
	CLEAR,
	
	MAX
}
public class UIPerformActionDataGroup
{
	private UIPerformActionData actionData;
	private Image imageObject;

	public UIPerformActionDataGroup(UIPerformActionData data, Image img)
	{
		actionData = data;
		imageObject = img;
	}

	public void SetImage(ActionImageType type)
	{
		if (type == ActionImageType.NONE || type == ActionImageType.MAX)
		{
			return;
		}

		var spr = (type == ActionImageType.NORMAL) ? actionData.normalSpr : actionData.clearSpr;
		imageObject.sprite = spr;
	}

	public void SetChecked(bool isTrigger)
	{
		actionData.isComplate = isTrigger;
	}

	public bool GetChecked()
	{
		return actionData.isComplate;
	}
}
