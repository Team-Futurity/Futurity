using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName ="UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	public List<StatusData> Status;
	[Space(10)]
	public List<StatusData> copyStatus;

	private void OnEnable()
	{
		copyStatus.Clear();
		
		CopyOrigin();
	}

	private void CopyOrigin()
	{
		foreach (var element in Status)
		{
			copyStatus.Add(new StatusData(element.name, element.value));
			
		}
	}

	public float GetStatus(StatusName name)
	{
		if (!ValidationStatus(name))
		{
			FDebug.Log($" {name}�� �ش��ϴ� Status�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}
		
		return copyStatus.First(x => x.name == name).value;
	}

	public void SetStatus(StatusName name, float value)
	{
		if (!ValidationStatus(name))
		{
			FDebug.Log($" {name}�� �ش��ϴ� Status�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}
		
		copyStatus.First(x => x.name == name).value = value;
	}
	
	public void SetStatus(StatusData data)
	{
		if (!ValidationStatus(data.name))
		{
			FDebug.Log($" {data.name}�� �ش��ϴ� Status�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}

		copyStatus.Find(x => x.name == data.name).value += data.value;
	}

	public void CalcSelfElement(StatusName name, float value)
	{
		copyStatus.First(x => x.name == name).value += value;
	}

	// �ش��ϴ� �����Ͱ� �ִ��� �����Ѵ�.
	private bool ValidationStatus(StatusName name)
	{
		foreach (var element in copyStatus)
		{
			if (element.name == name)
			{
				return true;
			}
		}

		return false;
	}
}
