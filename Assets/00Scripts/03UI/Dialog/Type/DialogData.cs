using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

	private int currentIndex;
	
	#region Unity Events

	[HideInInspector]
	public UnityEvent onInit;

	[HideInInspector]
	public UnityEvent onChanged;
	
	[HideInInspector]
	public UnityEvent onEnded;
	
	#endregion
	
	public void Init()
	{
		currentIndex = 0;
		currentData = dataList[currentIndex];
		
		onInit?.Invoke();
	}

	public bool Next()
	{
		currentIndex++;

		if (currentIndex >= dataList.Count)
		{
			onEnded?.Invoke();
			return true;
		}
		
		currentData = dataList[currentIndex];
		onChanged?.Invoke();
		
		return false;
	}

	public DialogDataGroup GetDialogDataGroup()
	{
		return currentData;
	}
	
}