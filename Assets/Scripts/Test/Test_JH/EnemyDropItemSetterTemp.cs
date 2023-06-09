using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemSetterTemp : MonoBehaviour
{
	[SerializeField]
	PartsRepositoryManager partsRepositoryManager;
	[SerializeField]
	List<ItemUIData> itemUiData;

	private void Start()
	{
		partsRepositoryManager = GameObject.Find("Player").GetComponent<PartsRepositoryManager>();
		partsRepositoryManager.SetEnemyItemUiDatas(itemUiData);
	}
}
