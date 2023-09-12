using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// 현재 Stage가 실행중인지 확인하기
	private bool isActive = false;

	// 모든 Area 데이터 가지고 있기
	[field: SerializeField]
	public List<AreaController> areaControllerList;

	[field: SerializeField]
	public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;


	// Stage 상태에 따른 Unity Event
	[HideInInspector]
	public UnityEvent<bool> OnStageActive;

	[HideInInspector]
	public UnityEvent OnStageStart;

	[HideInInspector]
	public UnityEvent OnStageEnd;

	// 현재 Area가 Start Directing을 가지고 있을 경우 실행한다. -> Play에서 처리해줄 것
	// 다음 씬으로 이동할 때, SetNextArea -> 네이밍 생각해볼 것.

	// Stage Controller의 역할
	// 씬 로딩 -> 로딩 완료 -> Play

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;
	}

	public void PlayStage()
	{
		OnStageStart?.Invoke();

		SetStageActive(true);
	}

	public void EndStage()
	{
		OnStageEnd?.Invoke();

		SetStageActive(false);
	}

	public void PauseStage()
	{
		SetStageActive(false);
	}


	public void SetNextArea()
	{
		CheckActive();

		if(currentAreaIndex + 1 >= maxAreaIndex)
		{
			return;
		}	

		currentAreaIndex++;
	}

	private void CheckActive()
	{
		if(!isActive)
		{
			FDebug.Log($"[{GetType()}] 현재 Active 상태가 아닙니다.");
			FDebug.Break();

			return;
		}
	}

	private void SetStageActive(bool isOn)
	{
		if (isActive == isOn)
		{
			return;
		}

		isActive = isOn;

		OnStageActive?.Invoke(isActive);
	}
}
