using System;
using System.Collections;
using UnityEngine;


public enum EStageType
{
	CHAPTER1_AREA1,
	CHAPTER1_AREA2,
	CHAPTER1_AREA3,
	BOSS_STAGE
}

public class StageMoveManager : MonoBehaviour
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

	public void MoveNextChapter()
	{
		currentSector++;

		if (currentSector == EStageType.BOSS_STAGE)
		{
			FadeManager.Instance.FadeIn(1.0f, (() => SceneLoader.Instance.LoadScene("BossChapter")));
		}
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
				TimelineManager.Instance.Chapter1_Area2_EnableCutScene(EChapter1_2.AREA2_ENTRYSCENE);
				break;
			
			default:
				return;
		}
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			playerTf.SetPositionAndRotation(startingPos[0].position, startingPos[0].rotation);
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			playerTf.SetPositionAndRotation(startingPos[1].position, startingPos[1].rotation);
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			playerTf.SetPositionAndRotation(startingPos[2].position, startingPos[2].rotation);
		}
		if(Input.GetKeyDown(KeyCode.F4))
		{
			FadeManager.Instance.FadeIn(1.0f, (() => SceneLoader.Instance.LoadScene("BossChapter")));
		}
	}
}
