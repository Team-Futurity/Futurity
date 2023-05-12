using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName ="UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	[field: SerializeField] public List<StatusData> Status { get; private set; }
	[HideInInspector] public List<StatusData> copyStatus;

	private void OnEnable()
	{
		copyStatus = Status;
	}

	public float GetStatus(StatusName name)
	{
		return copyStatus.First(x => x.name == name).value;
	}

	public void SetStatus(StatusName name, float value)
	{
		copyStatus.First(x => x.name == name).value = value;
	}

	// 해당하는 데이터가 있는지 검증한다.
	private void ValidationStatus(StatusName name)
	{
		
	}
}
