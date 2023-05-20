using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryContorller : Singleton<PartsRepositoryContorller>
{
	[Header("Parts�� �����ϰ� �����ϴ� ��ũ��Ʈ�Դϴ�.")]
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
	/// �ش� �Լ��� PartsData�� �����մϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	public void SettingPartsData(int settingNum)
	{ 
		if (settingNum < 3)
			repositoryPartsDatas[settingNum] = currentPartsData;
	}

	/// <summary>
	/// �ش� �Լ��� PartsData�� �����ɴϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	/// <returns>repositoryPartsDatas[settingNum]�� ��ȯ�մϴ�.</returns>
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
