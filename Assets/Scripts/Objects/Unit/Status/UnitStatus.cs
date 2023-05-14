using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	public List<StatusData> Status;
	[Space(10)] 
	[SerializeField] private List<StatusData> copyStatus;

	private void OnEnable()
	{
		if (copyStatus is not null)
		{
			copyStatus.Clear();
			CopyOrigin();
		}
	}

	#region Private

	private void CopyOrigin()
	{
		copyStatus = Status.ToList();
	}

	#endregion

	public void SetStatus(List<StatusData> statusDatas)
	{
		if (statusDatas is not null)
		{
			copyStatus = statusDatas.ToList();
		}
	}

	public void AddStatus(List<StatusData> statusDatas)
	{
		if (statusDatas is not null)
		{
			foreach (var status in statusDatas)
			{
				GetStatus(status.type).PlusValue(status.GetValue());
			}
		}
	}

	public void MinusStatus(List<StatusData> statusDatas)
	{
		if (statusDatas is not null)
		{
			foreach (var status in statusDatas)
			{
				GetStatus(status.type).MinusValue(status.GetValue());
			}
		}
	}

	// �ش� Status�� ������ �ִ��� Ȯ��
	public bool HasStatus(StatusType type)
	{
		foreach (var status in copyStatus)
		{
			if (status.type == type)
			{
				return true;
			}
		}

		return false;
	}

	// �ش� Status�� �����͸� �����´�.
	public StatusData GetStatus(StatusType type)
	{
		// ���� Status�� �ش� Status Type�� �ִ��� Ȯ��
		var hasStatus = HasStatus(type);

		if (!hasStatus)
		{
			FDebug.Log($"{type}�� �ش��ϴ� Status Key�� �������� �ʽ��ϴ�.");
			return null;
		}

		return copyStatus.Find(x => x.type == type);
	}
}