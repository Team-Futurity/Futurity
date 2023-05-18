using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryContorller : MonoBehaviour
{
	[Header("Parts�� �����ϰ� �����ϴ� ��ũ��Ʈ�Դϴ�.")]
	[Space(15)]


	[SerializeField]
	private PartsData currentPartsData;
	[SerializeField]
	private List<PartsData> repositoryPartsDatas;

	public void SelectedPartsData(PartsData partsData)
	{
		currentPartsData = partsData;
	}

	public void SettingPartsData(int settingNum)
	{
		if (settingNum < 3)
			repositoryPartsDatas[settingNum] = currentPartsData;
	}

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
