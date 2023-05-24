using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemSetterTemp : MonoBehaviour
{
	[SerializeField]
	List<ItemUIData> itemUiData;

	private void Start()
	{
		PartsRepositoryManager.Instance.SetEnemyItemUiDatas(itemUiData);
	}
}
