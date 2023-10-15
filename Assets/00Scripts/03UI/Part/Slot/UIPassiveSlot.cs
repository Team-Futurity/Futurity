using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPassiveSlot : MonoBehaviour
{
	public Image iconImage;
	public GameObject activateObj;

	public void SetSlot(Sprite partIcon)
	{
		iconImage.sprite = partIcon;
	}

	public void ActivateSlot(bool isOn)
	{
		activateObj.SetActive(isOn);
	}

	public void ClearSlot()
	{
		iconImage.sprite = null;
	}

}
