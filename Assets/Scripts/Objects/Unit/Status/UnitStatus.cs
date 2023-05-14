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

	// 해당 Status를 가지고 있는지 확인
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

	// 해당 Status의 데이터를 가져온다.
	public StatusData GetStatus(StatusType type)
	{
		// 현재 Status에 해당 Status Type이 있는지 확인
		var hasStatus = HasStatus(type);

		if (!hasStatus)
		{
			FDebug.Log($"{type}에 해당하는 Status Key가 존재하지 않습니다.");
			return null;
		}

		return copyStatus.Find(x => x.type == type);
	}
}