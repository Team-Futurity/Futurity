using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// Stage Controller�� �������ΰ�?
	private bool isRunning = false;

	[field: SerializeField] public List<AreaController> areaControllerList;

	[field: SerializeField] public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;

	[HideInInspector] public UnityEvent OnStageStart;

	[HideInInspector] public UnityEvent OnStageEnd;

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;

		SetUp();
		
		FadeManager.Instance.FadeOut(3f, new UnityAction( () =>
		{
			InitStage();
		}));
	}

	private void SetUp()
	{
		areaControllerList[currentAreaIndex].OnAreaClear.AddListener(() =>
		{
			areaControllerList[currentAreaIndex].OnAreaClear?.RemoveListener(ChangeToNextIndex);

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

		if (currentAreaIndex + 1 >= maxAreaIndex)
		{
			return;
		}

		currentAreaIndex++;
	}

	private void CheckStatus()
	{
		if (!isRunning)
		{
			FDebug.Log($"[{GetType()}] ���� Active ���°� �ƴմϴ�.");
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