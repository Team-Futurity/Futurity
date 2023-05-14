using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatusData
{
	public StatusData(StatusType type = StatusType.NONE, float value = .0f)
	{
		this.type = type;
		this.value = value;
	}

	public StatusData(StatusData data)
	{
		this.type = data.type;
		value = data.value;
	}
	
	public StatusType type;
	[SerializeField] private float value;

	public float GetValue()
	{
		return value;
	}

	public void SetValue(float value)
	{
		this.value = value;
	}
	
	public void PlusValue(float value)
	{
		this.value += value;
	}
	
	public void MinusValue(float value)
	{
		this.value -= value;
	}
	
}
