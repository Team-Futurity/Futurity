using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIPerformActionData
{
	public Sprite enableSpr;

	public Sprite disableSpr;

	public PlayerInput clearActionString;

	public bool isClear;

	public UIPerformActionData()
	{
		isClear = false;
	}
}
