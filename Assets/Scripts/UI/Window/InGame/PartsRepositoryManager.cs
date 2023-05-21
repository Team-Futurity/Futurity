using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts를 저장하고 관리하는 스크립트입니다.")]
	[Space(15)]


	[SerializeField]
	private PartsData currentPartsData;
	[SerializeField]
	private List<PartsData> enemyPartsDatas = new List<PartsData> { null, null, null };
	[SerializeField]
	private List<PartsData> repositoryPartsDatas = new List<PartsData> { null,null,null };


	/// <summary>
	/// 해당 함수는 currentPartsData를 할당합니다.
	/// </summary>
	/// <param name="settingNum">할당할 currnetPartsData</param>
	public void SetCurrentPartsData(PartsData partsData)
	{
		currentPartsData = partsData;
	}

	/// <summary>
	/// 해당 함수는 currnetPartsData에 있는 PartsData를 settingNum에 할당합니다.
	/// </summary>
	/// <param name="settingNum">설정할 데이터 저장소 넘버</param>
	public void SetPartsData(int settingNum)
	{ 
		if (settingNum < 3)
			repositoryPartsDatas[settingNum] = currentPartsData;
	}

	/// <summary>
	/// 해당 함수는 PartsData를 가져옵니다.
	/// </summary>
	/// <param name="settingNum">가져올 데이터 저장소 넘버</param>
	/// <returns>repositoryPartsDatas[settingNum]를 반환합니다.</returns>
	public PartsData GetRepositoryPartsData(int settingNum)
	{
		if (repositoryPartsDatas.Count > settingNum)
		{
			return repositoryPartsDatas[settingNum];
		}
		else
		{
			return null;
		}
	}


	/// <summary>
	/// 해당 함수는 enemyPartsDatas값을 할당합니다.
	/// </summary>
	/// <param name="enemyPartsDatas">설정할 PartsData의 List</param>
	public void SetEnemyDatas(List<PartsData> enemyPartsDatas)
	{
		this.enemyPartsDatas = enemyPartsDatas;
	}

	/// <summary>
	/// 해당 함수는 SetEnemyDatas를 가져옵니다.
	/// </summary>
	/// <param name="settingNum">가져올 데이터 저장소 넘버</param>
	/// <returns>repositoryPartsDatas[settingNum]를 반환합니다.</returns>
	public PartsData GetEnemyData(int settingNum)
	{
		if (enemyPartsDatas.Count > settingNum)
		{
			return enemyPartsDatas[settingNum];
		}
		else
		{
			return null;
		}
	}
}
