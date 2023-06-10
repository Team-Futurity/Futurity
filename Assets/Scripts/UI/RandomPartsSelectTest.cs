using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPartsSelectTest : MonoBehaviour
{
	[SerializeField]
	private List<PartsSelectButton> partsSelectButton;

	[SerializeField]
	private List<ItemUIData> itemUIDataList;

	public void RandomChangeItemData()
	{
		int randomIndex = Random.Range(0, itemUIDataList.Count);


		for (int i = 0; i < partsSelectButton.Count; i++)
		{
			partsSelectButton[i].SetItemUIData(itemUIDataList[randomIndex]);
		}
	}
}
