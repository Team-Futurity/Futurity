using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketController : MonoBehaviour
{
	private Image itemImage;

	
	[SerializeField] private ItemUIData itemUIData;

	void Awake()
	{
		ResetItemUIData();
	}

	/// <summary>
	/// ItemUIData를 새롭게 할당합니다.
	/// </summary>
	public void SetItemUIData(ItemUIData newItemData)
	{
		itemUIData = newItemData;

		ResetItemUIData();
	}

	/// <summary>
	/// ItemUIData에 맞추어서 Image.Sprite를 변경합니다.
	/// </summary>
	private void ResetItemUIData()
	{
		if (itemUIData is not null)
		{
			itemImage.sprite = itemUIData.ItemSprite;

			if (itemImage.sprite is not null)
			{
				itemImage.color = Color.white;
			}
			else
			{
				itemImage.color = Color.clear;
			}
		}
		else
		{
			itemImage.sprite = null;
		}
	}
}
