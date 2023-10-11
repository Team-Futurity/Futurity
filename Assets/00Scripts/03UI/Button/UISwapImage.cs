using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UISwap
{
	public Image spriteObject;

	[Space(10)]
	public Sprite normalSpr;
	public Sprite swapSpr;

	private bool isSwap = false;

	public void Swap(bool isOn)
	{
		isSwap = isOn;

		spriteObject.sprite = (isSwap == false) ? normalSpr : swapSpr;
	}
}