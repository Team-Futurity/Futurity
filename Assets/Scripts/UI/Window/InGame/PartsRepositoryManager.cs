using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRepositoryManager : Singleton<PartsRepositoryManager>
{
	[Header("Parts�� �����ϰ� �����ϴ� ��ũ��Ʈ�Դϴ�.")]
	[Space(15)]


	[SerializeField]
	private ItemUIData currentItemUiData;
	[SerializeField]
	[Tooltip ("�ش� ������ enemy�� ��� ���� player�� ������ �� �ִ� PartsData�� ����Ʈ�Դϴ�.")]
	private List<ItemUIData> enemyItemUiDatas = new List<ItemUIData> { null, null, null };
	[SerializeField]
	[Tooltip("�ش� ������ player�� ���� ������ PartsData�� ����Ʈ�Դϴ�.")]
	private List<ItemUIData> repositoryItemUiDatas = new List<ItemUIData> { null,null,null };


	/// <summary>
	/// �Է¹��� ItemUIData�� currentItemUiData�� �Ҵ��ϴ� �Լ��Դϴ�.
	/// </summary>
	/// <param name="itemUiData">�Ҵ��� ItemUIData</param>
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
	/// currentItemUiData�� repositoryItemUiDatas�� Ư�� ��ġ�� �Ҵ��ϴ� �Լ��Դϴ�.
	/// </summary>
	/// <param name="settingNum">ItemUIData�� �Ҵ��� ��ġ</param>
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
	/// �Է¹��� ItemUIData�� repositoryItemUiDatas�� Ư�� ��ġ�� �Ҵ��ϴ� �Լ��Դϴ�.
	/// </summary>
	/// <param name="newItemUiData">�Ҵ��� ItemUIData</param>
	/// <param name="settingNum">ItemUIData�� �Ҵ��� ��ġ</param>
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
	/// repositoryItemUiDatas�� Ư�� ��ġ���� ItemUIData�� �������� �Լ��Դϴ�.
	/// </summary>
	/// <param name="settingNum">ItemUIData�� ������ ��ġ</param>
	/// <returns>������ ItemUIData</returns>
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
	/// �Է¹��� List<ItemUIData>�� enemyItemUiDatas�� �Ҵ��ϴ� �Լ��Դϴ�. ����Ʈ�� null�̰ų� 3�� �̻��� �׸��� ������ ������ ���� �޽����� ����մϴ�.
	/// </summary>
	/// <param name="enemyItemUiDatas">�Ҵ��� List<ItemUIData></param>
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
	/// �Է¹��� ItemUIData�� enemyItemUiDatas�� Ư�� ��ġ�� �Ҵ��ϴ� �Լ��Դϴ�. ItemUIData�� �ε����� ��ȿ���� ���� ��� ���� �޽����� ����մϴ�.
	/// </summary>
	/// <param name="newEnemyItemUiData">�Ҵ��� ItemUIData</param>
	/// <param name="settingNum">ItemUIData�� �Ҵ��� ��ġ</param>
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
	/// enemyItemUiDatas�� Ư�� ��ġ���� ItemUIData�� �������� �Լ��Դϴ�. �ε����� ��ȿ���� ���� ��� ���� �޽����� ����ϰ� null�� ��ȯ�մϴ�.
	/// </summary>
	/// <param name="settingNum">ItemUIData�� ������ ��ġ</param>
	/// <returns>������ ItemUIData</returns>
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
