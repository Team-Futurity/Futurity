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

	// Start is called before the first frame update
	void Start()
	{

		PartsRepositoryManager partsRepository = GameObject.Find("Player").GetComponent<PartsRepositoryManager>();
		if (partsRepository.GetRepositoryPartsData(itemNum) is not null)
		{
			itemUIData = partsRepository.GetRepositoryPartsData(itemNum);

			if (itemUIData is not null)
			{
				itemImage.sprite = itemUIData.ItemSprite;

				if (itemImage.sprite is not null)
				{
					itemImage.color = new Color(255, 255, 255, 255);
				}
				else
				{
					itemImage.color = new Color(255, 255, 255, 0);
				}
			}
			else
			{
				itemImage.sprite = null;
			}
		}
		{
			itemUIData = null;
		}
	}

	public void ResetItemUIDatas()
	{
		itemUIData = GameObject.Find("Player").GetComponent<PartsRepositoryManager>().GetRepositoryPartsData(itemNum);
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
