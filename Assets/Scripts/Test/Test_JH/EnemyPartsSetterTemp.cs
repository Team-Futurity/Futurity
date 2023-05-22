using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartsSetterTemp : MonoBehaviour
{
	[SerializeField]
	List<PartsData> partsDatas;

	private void Start()
	{
		PartsRepositoryManager.Instance.SetEnemyDatas(partsDatas);
	}
}
