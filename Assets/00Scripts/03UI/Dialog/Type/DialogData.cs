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
	private DialogDataGroup nextData;

	private int currentIndex;
	private int nextIndex;

	private bool onLastData;

	public void Init()
	{
		currentData.Reset();
		currentIndex = 0;
		nextIndex = currentIndex + 1;
		onLastData = false;
		
		currentData = dataList[currentIndex];
		nextData = dataList[nextIndex];
	}

	// Index, Dialog Data
	public (int, DialogDataGroup) GetCurrentData()
	{
		return (currentIndex, currentData);
	}

	public void NextDialog()
	{
		if (onLastData)
			return;

		if(currentIndex >= dataList.Count - 1)
		{
			onLastData = true;
		}

		currentData = nextData;
		currentIndex = nextIndex;

		nextIndex++;

		if (nextIndex < dataList.Count)
		{
			nextData = dataList[nextIndex];
		}
	}

	public bool GetLastData()
	{
		return onLastData;
	}
}