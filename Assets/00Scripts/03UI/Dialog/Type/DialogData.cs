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

	private bool isActive = false;
	
	#region Unity Events

	[Space(10)]
	public UnityEvent onInit;
	public UnityEvent onChanged;
	public UnityEvent onEnded;
	
	#endregion
	
	public void Init()
	{
		currentIndex = 0;
		currentData = dataList[currentIndex];
		
		onInit?.Invoke();
		isActive = true;
	}

	public bool Next()
	{
		if(!isActive)
		{
			return true;
		}

		currentIndex++;

		if (currentIndex >= dataList.Count && isActive)
		{
			onEnded?.Invoke();
			isActive = false;

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