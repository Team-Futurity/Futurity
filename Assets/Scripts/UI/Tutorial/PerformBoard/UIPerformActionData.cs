using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIPerformActionData
{
	public Sprite normalSpr;
	public Sprite clearSpr;

	public PlayerInputEnum conditionAction;
	public bool isComplate;
}
