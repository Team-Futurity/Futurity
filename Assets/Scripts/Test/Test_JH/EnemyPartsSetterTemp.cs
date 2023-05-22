using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartsSetterTemp : MonoBehaviour
{
	[SerializeField]
	List<ItemUIData> ItemUIDatas;

	private void Start()
	{
		PartsRepositoryManager.Instance.SetEnemyDatas(ItemUIDatas);
	}
}
