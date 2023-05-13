using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatusData
{
	public StatusData(StatusName name, float value)
	{
		this.name = name;
		this.value = value;
	}
	
	public StatusName name;
	public float value;
}
