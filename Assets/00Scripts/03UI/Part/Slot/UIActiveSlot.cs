using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveSlot : MonoBehaviour
{
	public Image iconImage;

	public Sprite normalSpr;
	public Sprite swapSpr;

	public void SetSlot()
	{
		iconImage.sprite = swapSpr;
	}

	public void ClearSlot()
	{
		iconImage.sprite = normalSpr;
	}
}
