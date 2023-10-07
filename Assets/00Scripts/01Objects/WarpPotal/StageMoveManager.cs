using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStageType
{
	CHAPTER1_AREA1,
	CHAPTER1_AREA2,
	CHAPTER1_AREA3,
	BOSS_STAGE
}

public class StageMoveManager : Singleton<StageMoveManager>
{
	[Header("Component")] 
	[SerializeField] private GameObject interactionUI;
	private Transform playerTf;
	
	[Header("현재 플레이어가 있는 구역")] 
	[ReadOnly(false)] public EStageType currentSector;

	[Header("각 구역 출구 콜라이더")] 
	[SerializeField] private GameObject[] exitCollider;

	[Header("각 구역 시작 위치")] 
	[SerializeField] private Transform[] startingPos;

	private bool isFadeDone = false;

	private void Start()
	{
		currentSector = EStageType.CHAPTER1_AREA1;
		playerTf = GameObject.FindWithTag("Player").transform;
	}

	public void EnableExitCollider()
	{
		exitCollider[(int)currentSector].SetActive(true);
	}

	public void SetActiveInteractionUI(bool enable)
	{
		interactionUI.SetActive(enable);
	}

	public void NonFadeOutMoveSector()
	{
		playerTf.SetPositionAndRotation(startingPos[(int)currentSector + 1].position, startingPos[(int)currentSector + 1].rotation);
		currentSector++;
	}
	
	public void MoveNextSector()
	{
		StartCoroutine(WaitForFadeEnd());
	}

	private IEnumerator WaitForFadeEnd()
	{
		FadeManager.Instance.FadeIn(1.0f, (() => isFadeDone = true));

		while (isFadeDone == false)
		{
			yield return null;
		}

		isFadeDone = false;
		playerTf.SetPositionAndRotation(startingPos[(int)currentSector + 1].position, startingPos[(int)currentSector + 1].rotation);
		currentSector++;
		
		FadeManager.Instance.FadeOut();
		CheckCutSceneEnable();
	}

	private void CheckCutSceneEnable()
	{
		switch (currentSector)
		{
			case EStageType.CHAPTER1_AREA3:
				TimelineManager.Instance.EnableCutScene(ECutScene.AREA3_ENTRYCUTSCENE);
				break;
			
			default:
				return;
		}
	}
	
}
