using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts를 저장하고 관리하는 스크립트입니다.")]
	[Space(15)]


	[SerializeField]
	private ItemUIData currentItemUiData;
	[SerializeField]
	[Tooltip ("해당 변수는 enemy가 사망 이후 player가 선택할 수 있는 PartsData의 리스트입니다.")]
	private List<ItemUIData> enemyItemUiDatas = new List<ItemUIData> { null, null, null };
	[SerializeField]
	[Tooltip("해당 변수는 player가 변경 가능한 PartsData의 리스트입니다.")]
	private List<ItemUIData> repositoryItemUiDatas = new List<ItemUIData> { null,null,null };


	/// <summary>
	/// 입력받은 ItemUIData를 currentItemUiData에 할당하는 함수입니다.
	/// </summary>
	/// <param name="itemUiData">할당할 ItemUIData</param>
	public void SetCurrentPartsData(ItemUIData itemUiData)
	{
		if (itemUiData != null)
		{
			currentItemUiData = itemUiData;
		}
		else
		{
			Debug.LogError("Input ItemUIData is null");
		}
	}


	/// <summary>
	/// currentItemUiData를 repositoryItemUiDatas의 특정 위치에 할당하는 함수입니다.
	/// </summary>
	/// <param name="settingNum">ItemUIData를 할당할 위치</param>
	public void SetPartsData(int settingNum)
	{
		if (settingNum >= 0 && settingNum < repositoryItemUiDatas.Count)
		{ 
			repositoryItemUiDatas[settingNum] = currentItemUiData;
		}
		else
		{
			Debug.LogError("Invalid setting number");
		}
	}
	/// <summary>
	/// 입력받은 ItemUIData를 repositoryItemUiDatas의 특정 위치에 할당하는 함수입니다.
	/// </summary>
	/// <param name="newItemUiData">할당할 ItemUIData</param>
	/// <param name="settingNum">ItemUIData를 할당할 위치</param>
	public void SetPartsData(ItemUIData newItemUiData, int settingNum)
	{
		if (newItemUiData != null && settingNum >= 0 && settingNum < repositoryItemUiDatas.Count)
		{
			repositoryItemUiDatas[settingNum] = newItemUiData;
		}
		else
		{
			Debug.LogError("Input ItemUIData is null or invalid setting number");
		}
	}


	/// <summary>
	/// repositoryItemUiDatas의 특정 위치에서 ItemUIData를 가져오는 함수입니다.
	/// </summary>
	/// <param name="settingNum">ItemUIData를 가져올 위치</param>
	/// <returns>가져온 ItemUIData</returns>
	public ItemUIData GetRepositoryPartsData(int settingNum)
	{
		if (settingNum >= 0 && repositoryItemUiDatas.Count > settingNum)
		{ 
			return repositoryItemUiDatas[settingNum];
		}
		else
		{
			Debug.LogError("Invalid setting number");
			return null;
		}
	}


	/// <summary>
	/// 입력받은 List<ItemUIData>를 enemyItemUiDatas에 할당하는 함수입니다. 리스트가 null이거나 3개 이상의 항목을 가지고 있으면 에러 메시지를 출력합니다.
	/// </summary>
	/// <param name="enemyItemUiDatas">할당할 List<ItemUIData></param>
	public void SetEnemyItemUiDatas(List<ItemUIData> enemyItemUiDatas)
	{
		if (enemyItemUiDatas != null && enemyItemUiDatas.Count <= 3)
		{
			this.enemyItemUiDatas = new List<ItemUIData>(enemyItemUiDatas);
		}
		else
		{
			Debug.LogError("Input list is null or has too many items");
		}
	}

	/// <summary>
	/// 입력받은 ItemUIData를 enemyItemUiDatas의 특정 위치에 할당하는 함수입니다. ItemUIData나 인덱스가 유효하지 않은 경우 에러 메시지를 출력합니다.
	/// </summary>
	/// <param name="newEnemyItemUiData">할당할 ItemUIData</param>
	/// <param name="settingNum">ItemUIData를 할당할 위치</param>
	public void SetEnemyData(ItemUIData newEnemyItemUiData, int settingNum)
	{
		if (newEnemyItemUiData != null && settingNum >= 0 && settingNum < enemyItemUiDatas.Count)
		{
			enemyItemUiDatas[settingNum] = newEnemyItemUiData;
		}
		else
		{
			Debug.LogError("Input ItemUIData is null or invalid setting number");
		}
	}


	/// <summary>
	/// enemyItemUiDatas의 특정 위치에서 ItemUIData를 가져오는 함수입니다. 인덱스가 유효하지 않은 경우 에러 메시지를 출력하고 null을 반환합니다.
	/// </summary>
	/// <param name="settingNum">ItemUIData를 가져올 위치</param>
	/// <returns>가져온 ItemUIData</returns>
	public ItemUIData GetEnemyData(int settingNum)
	{
		if (enemyItemUiDatas.Count > settingNum)
		{
			return enemyItemUiDatas[settingNum];
		}
		else
		{
			return null;
		}
	}
}
