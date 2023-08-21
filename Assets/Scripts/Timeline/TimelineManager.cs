using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : Singleton<TimelineManager>
{
	[Header("컷신 목록")] 
	[SerializeField] private GameObject[] cutSceneList;
	
	public enum ECutScene
	{
		Stage1_EntryCutScene = 0,
		PlayerDeathCutScene = 1
	}

	private IEnumerator testCoroutine;
	
	private void Start()
	{
		// 컷신을 재생하는 함수가 다른곳에서 불릴 때까지 해당 지점에서 1구역 진입 연출을 시작합니다.
		// 로딩이 끝난 후 실행할 수 있도록 의도적으로 함수 실행시간을 지연시킵니다.
		Time.timeScale = 0.0f;
		testCoroutine = DelayCutScene(ECutScene.Stage1_EntryCutScene);
		StartCoroutine(testCoroutine);
	}

	public void EnableCutScene(ECutScene cutScene)
	{
		cutSceneList[(int)cutScene].SetActive(true);
	}

	private IEnumerator DelayCutScene(ECutScene cutScene)
	{
		yield return new WaitForSecondsRealtime(5.0f);
		EnableCutScene(cutScene);
	}
}
