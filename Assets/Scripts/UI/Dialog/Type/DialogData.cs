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

	public void Reset()
	{
		talker_Eng = "";
		talker_Kor = "";
		descripton = "";
	}
}

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/New Dialog", order = 0)]
public class DialogData : ScriptableObject
{
	public List<DialogDataGroup> dataList;

	private DialogDataGroup currentData;

	private int currentIndex;

	private bool onLastData;

	public void Init()
	{
		currentData.Reset();
		currentIndex = 0;
		onLastData = false;
		
		currentData = dataList[currentIndex];
	}

	// Index, Dialog Data
	public (int, DialogDataGroup) GetCurrentData()
	{
		return (currentIndex, currentData);
	}

	public void NextDialog()
	{
		if (onLastData)
		{
			return;
		}
		
		currentData = dataList[++currentIndex];

		if (currentIndex >= dataList.Count - 1)
		{
			onLastData = true;
		}
	}

	public bool GetLastData()
	{
		return onLastData;
	}
}