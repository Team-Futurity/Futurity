using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts를 저장하고 관리하는 스크립트입니다.")]
	[Space(15)]


	[SerializeField]
	private ItemUIData currentItemUIData;
	[SerializeField]
	private List<ItemUIData> enemyItemUIDatas = new List<ItemUIData> { null, null, null };
	[SerializeField]
	private List<ItemUIData> repositoryItemUIDatas = new List<ItemUIData> { null,null,null };


	/// <summary>
	/// 해당 함수는 currentItemUIData를 할당합니다.
	/// </summary>
	/// <param name="settingNum">할당할 currnetItemUIData</param>
	public void SetCurrentItemUIData(ItemUIData itemUIData)
	{
		currentItemUIData = itemUIData;
	}

	/// <summary>
	/// 해당 함수는 currnetItemUIData에 있는 ItemUIData를 settingNum에 할당합니다.
	/// </summary>
	/// <param name="settingNum">설정할 데이터 저장소 넘버</param>
	public void SetItemUIData(int settingNum)
	{ 
		if (settingNum < 3)
			repositoryItemUIDatas[settingNum] = currentItemUIData;
	}

	/// <summary>
	/// 해당 함수는 ItemUIData를 가져옵니다.
	/// </summary>
	/// <param name="settingNum">가져올 데이터 저장소 넘버</param>
	/// <returns>repositoryItemUIDatas[settingNum]를 반환합니다.</returns>
	public ItemUIData GetRepositoryItemUIData(int settingNum)
	{
		if (repositoryItemUIDatas.Count > settingNum)
		{
			return repositoryItemUIDatas[settingNum];
		}
		else
		{
			return null;
		}
	}


	/// <summary>
	/// 해당 함수는 enemyItemUIDatas값을 할당합니다.
	/// </summary>
	/// <param name="enemyItemUIDatas">설정할 ItemUIData의 List</param>
	public void SetEnemyDatas(List<ItemUIData> enemyItemUIDatas)
	{
		this.enemyItemUIDatas = enemyItemUIDatas;
	}

	/// <summary>
	/// 해당 함수는 SetEnemyDatas를 가져옵니다.
	/// </summary>
	/// <param name="settingNum">가져올 데이터 저장소 넘버</param>
	/// <returns>repositoryItemUIDatas[settingNum]를 반환합니다.</returns>
	public ItemUIData GetEnemyData(int settingNum)
	{
		if (enemyItemUIDatas.Count > settingNum)
		{
			return enemyItemUIDatas[settingNum];
		}
		else
		{
			return null;
		}
	}
}
