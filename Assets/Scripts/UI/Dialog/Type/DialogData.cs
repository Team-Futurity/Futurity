using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DialogDataGroup
{
	public string talker_Eng;
	public string talker_Kor;
	public string descripton; 
}

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/New Dialog", order = 0)]
public class DialogData : ScriptableObject
{
	public List<DialogDataGroup> dataList;

	private DialogDataGroup currentData;
	private DialogDataGroup nextData;

	private int currentIndex;

	public void Init()
	{
		currentIndex = 0;
		currentData = dataList[currentIndex];
		
		if (dataList.Count > 1)
		{
			nextData = dataList[currentIndex + 1];
		}
	}

	// Index, Dialog Data
	public (int, DialogDataGroup) GetCurrentData()
	{ 
		return (currentIndex, currentData);
	}

	public void NextDialog()
	{
		currentData = nextData;

		nextData = dataList[++currentIndex];
	}
}
