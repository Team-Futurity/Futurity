using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryContorller : Singleton<PartsRepositoryContorller>
{
	[Header("Parts를 저장하고 관리하는 스크립트입니다.")]
	[Space(15)]


	[SerializeField]
	private PartsData currentPartsData;
	[SerializeField]
	private List<PartsData> repositoryPartsDatas = new List<PartsData> { null,null,null };

	public void SelectedPartsData(PartsData partsData)
	{
		currentPartsData = partsData;
	}

	/// <summary>
	/// 해당 함수는 PartsData를 설정합니다.
	/// </summary>
	/// <param name="settingNum">설정할 데이터 저장소 넘버</param>
	public void SettingPartsData(int settingNum)
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
}
