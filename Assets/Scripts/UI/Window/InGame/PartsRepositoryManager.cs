using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts�� �����ϰ� �����ϴ� ��ũ��Ʈ�Դϴ�.")]
	[Space(15)]


	[SerializeField]
	private ItemUIData currentItemUIData;
	[SerializeField]
	private List<ItemUIData> enemyItemUIDatas = new List<ItemUIData> { null, null, null };
	[SerializeField]
	private List<ItemUIData> repositoryItemUIDatas = new List<ItemUIData> { null,null,null };


	/// <summary>
	/// �ش� �Լ��� currentItemUIData�� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="settingNum">�Ҵ��� currnetItemUIData</param>
	public void SetCurrentItemUIData(ItemUIData itemUIData)
	{
		currentItemUIData = itemUIData;
	}

	/// <summary>
	/// �ش� �Լ��� currnetItemUIData�� �ִ� ItemUIData�� settingNum�� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	public void SetItemUIData(int settingNum)
	{ 
		if (settingNum < 3)
			repositoryItemUIDatas[settingNum] = currentItemUIData;
	}

	/// <summary>
	/// �ش� �Լ��� ItemUIData�� �����ɴϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	/// <returns>repositoryItemUIDatas[settingNum]�� ��ȯ�մϴ�.</returns>
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
	/// �ش� �Լ��� enemyItemUIDatas���� �Ҵ��մϴ�.
	/// </summary>
	/// <param name="enemyItemUIDatas">������ ItemUIData�� List</param>
	public void SetEnemyDatas(List<ItemUIData> enemyItemUIDatas)
	{
		this.enemyItemUIDatas = enemyItemUIDatas;
	}

	/// <summary>
	/// �ش� �Լ��� SetEnemyDatas�� �����ɴϴ�.
	/// </summary>
	/// <param name="settingNum">������ ������ ����� �ѹ�</param>
	/// <returns>repositoryItemUIDatas[settingNum]�� ��ȯ�մϴ�.</returns>
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
