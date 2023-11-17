using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveSlot : MonoBehaviour
{
	public Image iconImage;

	public Sprite normalSpr;
	public Sprite swapSpr;

	[Header("Alpha")]
	public Sprite normalBasic;
	public Sprite swapBasic;

	[Header("Beta")]
	public Sprite normalBeta;
	public Sprite swapBeta;

	[Header("Color")]
	public Color normalColor;
	public Color activeColor;

	public void SetSlot()
	{
		iconImage.sprite = swapSpr;
		iconImage.color = activeColor;
	}

	public void ClearSlot()
	{
		iconImage.sprite = normalSpr;
		iconImage.color = normalColor;
	}

	public void BetaSet()
	{
		normalSpr = normalBeta;
		swapSpr = swapBeta;

		iconImage.sprite = normalSpr;
	}

	public void BasicSet()
	{
		normalSpr = normalBasic;
		swapSpr = swapBasic;
		
		iconImage.sprite = normalSpr;
	}
}
