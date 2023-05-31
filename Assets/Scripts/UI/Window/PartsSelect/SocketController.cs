using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketController : MonoBehaviour
{
	[SerializeField]
	private int itemNum = 0;

	[SerializeField]
	private Image itemImage;
	private ItemUIData itemUIData;



	public void ResetItemUIDatas(ItemUIData setItemData)
	{
		itemUIData = setItemData;

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
			itemImage.color = Color.clear;
		}
	}
	public void ResetItemUIDatas()
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
			itemImage.color = Color.clear;
		}
	}
}
