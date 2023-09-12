using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// 현재 Stage가 실행중인지 확인하기
	private bool isRunning = false;

	// 모든 Area 데이터 가지고 있기
	[field: SerializeField]
	public List<AreaController> areaControllerList;

	[field: SerializeField]
	public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;

	[HideInInspector]
	public UnityEvent OnStageStart;

	[HideInInspector]
	public UnityEvent OnStageEnd;

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;

		SetUp();
	}

	private void SetUp()
	{
		areaControllerList[currentAreaIndex].OnAreaClear.AddListener(() =>
	   {
		   ChangeToNextIndex();
	   });
	}

	public void InitStage()
	{
		areaControllerList[currentAreaIndex].InitArea();

		SetRunning(true);
	}

	public void PlayStage()
	{
		areaControllerList[currentAreaIndex].PlayArea();
	}

	public void EndStage()
	{
		SetRunning(false);

		PauseStage();
	}

	public void PauseStage()
	{
		areaControllerList[currentAreaIndex].StopArea();
	}

	private void ChangeToNextIndex()
	{
		CheckStatus();

		if(currentAreaIndex + 1 >= maxAreaIndex)
		{
			return;
		}	

		currentAreaIndex++;
	}

	private void CheckStatus()
	{
		if(!isRunning)
		{
			FDebug.Log($"[{GetType()}] 현재 Active 상태가 아닙니다.");
			FDebug.Break();

			return;
		}
	}

	private void SetRunning(bool isOn)
	{
		if (isRunning == isOn)
		{
			return;
		}

		isRunning = isOn;
	}
}
