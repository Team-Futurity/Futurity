using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPassiveSlot : MonoBehaviour
{
	public Image iconImage;
	public Image activateImage;

	public void SetSlot(Sprite partIcon)
	{
		iconImage.sprite = partIcon;
		iconImage.enabled = true;
	}

	public void SetActivateImage(bool isOn)
	{
		activateImage.enabled = isOn;
	}

	public void ClearSlot()
	{
		iconImage.enabled = false;
		SetActivateImage(false);
	}
}
