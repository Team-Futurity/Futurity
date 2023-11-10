using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveSlot : MonoBehaviour
{
	public Image iconImage;

	public Sprite normalSpr;
	public Sprite swapSpr;

	public Sprite normalBasic;
	public Sprite swapBasic;

	[Header("Beta")]
	public Sprite normalBeta;
	public Sprite swapBeta;

	public void SetSlot()
	{
		iconImage.sprite = swapSpr;
	}

	public void ClearSlot()
	{
		iconImage.sprite = normalSpr;
	}

	public void BetaSet()
	{
		normalSpr = normalBeta;
		swapSpr = swapBeta;
	}

	public void BasicSet()
	{
		normalSpr = normalBasic;
		swapSpr = swapBasic;
	}
}
