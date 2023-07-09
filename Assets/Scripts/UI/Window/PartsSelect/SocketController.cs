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
	/// ItemUIData�� ���Ӱ� �Ҵ��մϴ�.
	/// </summary>
	public void SetItemUIData(ItemUIData newItemData)
	{
		itemUIData = newItemData;

		ResetItemUIData();
	}

	/// <summary>
	/// ItemUIData�� ���߾ Image.Sprite�� �����մϴ�.
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
