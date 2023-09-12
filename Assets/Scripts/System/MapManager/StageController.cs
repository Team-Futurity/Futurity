using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// 모든 Area 데이터 가지고 있기
	[field: SerializeField]
	public List<AreaController> areaControllerList;

	// 현재 Area의 Index
	[field: SerializeField]
	public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;

	// 현재 Area의 Init, Play, Stop 메서드 가지고 있기
	// Init : 데이터 초기화
	// Play : 즉각적인 게임 Play
	// Stop : 게임 일시 중지 -> UI 제외

	// 현재 Area가 Start Directing을 가지고 있을 경우 실행한다. -> Play에서 처리해줄 것
	// 다음 씬으로 이동할 때, SetNextArea -> 네이밍 생각해볼 것.

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;
	}

	public void SetNextArea()
	{
		// 인덱스 증가
	}

	public void InitArea()
	{
	}

	public void PlayArea()
	{

	}

	public void StopArea()
	{

	}
}
