using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts�� �����ϰ� �����ϴ� ��ũ��Ʈ�Դϴ�.")]
	[Space(15)]


	[SerializeField]
	private PartsData currentPartsData;
	[SerializeField]
	[Tooltip ("�ش� ������ enemy�� ��� ���� player�� ������ �� �ִ� PartsData�� ����Ʈ�Դϴ�.")]
	private List<PartsData> enemyPartsDatas = new List<PartsData> { null, null, null };
	[SerializeField]
	[Tooltip("�ش� ������ player�� ���� ������ PartsData�� ����Ʈ�Դϴ�.")]
	private List<PartsData> repositoryPartsDatas = new List<PartsData> { null,null,null };


	/// <summary>
	/// �ش� �Լ��� currentPartsData�� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="settingNum">�Ҵ��� currnetPartsData</param>
	public void SetCurrentPartsData(PartsData partsData)
	{
		currentPartsData = partsData;
	}


	/// <summary>
	/// �ش� �Լ��� currnetPartsData�� �ִ� PartsData�� settingNum�� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	public void SetPartsData(int settingNum)
	{ 
		if (settingNum < 3)
			repositoryPartsDatas[settingNum] = currentPartsData;
	}
	/// <summary>
	/// �ش� �Լ��� currnetPartsData�� �ִ� PartsData�� settingNum�� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	/// <param name="newPartsData">������ PartsData �ѹ�</param>
	public void SetPartsData(PartsData newPartsData, int settingNum)
	{
		if (settingNum < 3)
			repositoryPartsDatas[settingNum] = newPartsData;
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


	/// <summary>
	/// �ش� �Լ��� enemyPartsDatas�� List�� �����մϴ�.
	/// </summary>
	/// <param name="enemyPartsDatas">������ PartsData�� List</param>
	public void SetEnemyDatas(List<PartsData> enemyPartsDatas)
	{
		this.enemyPartsDatas = enemyPartsDatas;
	}

	/// <summary>
	/// �ش� �Լ��� enemyPartsData���� �����մϴ�.
	/// </summary>
	/// <param name="newEnemyPartsData">������ PartsData�� List</param>
	public void SetEnemyData(PartsData newEnemyPartsData, int settingNum)
	{
		if (settingNum < 3)
			enemyPartsDatas[settingNum] = newEnemyPartsData;
	}


	/// <summary>
	/// �ش� �Լ��� SetEnemyDatas�� �����ɴϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	/// <returns>repositoryPartsDatas[settingNum]�� ��ȯ�մϴ�.</returns>
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
