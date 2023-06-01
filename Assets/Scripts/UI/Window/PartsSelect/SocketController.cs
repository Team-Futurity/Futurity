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

	[SerializeField]
	private ItemUIData itemUIData;


	void Start()
	{
		ResetItemUIData();
	}

	public void SetItemUIData()
	{
		itemUIData = new ItemUIData();
	}

	/// <summary>
	/// ItemUIData에 맞추어서 Image.Sprite를 변경함
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
