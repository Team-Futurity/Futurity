using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	[SerializeField] private List<StatusData> status;
	[Space(10)] 
	[SerializeField] private List<StatusData> copyStatus;
	
	
	private void OnEnable()
	{
		CopyOrigin();
	}

	#region Private

	private void CopyOrigin()
	{
		if (copyStatus is not null)
		{
			copyStatus.Clear();

			foreach (var stat in status)
			{
				StatusData newStatus = new StatusData();

				newStatus.type = stat.type;
				newStatus.SetValue(stat.GetValue());

				copyStatus.Add(newStatus);
			}
		}
	}

	#endregion

	public void AutoGenerator()
	{
		var statusTypeList = Enum.GetValues(typeof(StatusType)).Cast<StatusType>();
		
		foreach (var statusType in statusTypeList)
		{
			if (!HasStatus(statusType))
			{
				Debug.Log($"{statusType}");
				
				if (statusType is StatusType.NONE or StatusType.MAX)
					continue;
				
				status.Add(new StatusData(statusType));
			}
		}
		CopyOrigin();
	}

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