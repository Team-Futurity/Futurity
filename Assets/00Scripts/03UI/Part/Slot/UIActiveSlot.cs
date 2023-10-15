using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveSlot : MonoBehaviour
{
	public Image iconImage;

	public void SetSlot(Sprite partIcon)
	{
		iconImage.sprite = partIcon;
	}

	public void ClearSlot()
	{
		iconImage.sprite = null;
	}
}
