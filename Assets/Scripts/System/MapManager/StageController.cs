using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// ���� Stage�� ���������� Ȯ���ϱ�
	private bool isRunning = false;

	// ��� Area ������ ������ �ֱ�
	[field: SerializeField]
	public List<AreaController> areaControllerList;

	[field: SerializeField]
	public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;

	// Stage ���¿� ���� Unity Event
	[HideInInspector]
	public UnityEvent<bool> OnStageActive;

	[HideInInspector]
	public UnityEvent OnStageStart;

	[HideInInspector]
	public UnityEvent OnStageEnd;

	// ���� Area�� Start Directing�� ������ ���� ��� �����Ѵ�. -> Play���� ó������ ��
	// ���� ������ �̵��� ��, SetNextArea -> ���̹� �����غ� ��.

	// Stage Controller�� ����
	// �� �ε� -> �ε� �Ϸ� -> Play

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;
	}

	public void InitStage()
	{
		areaControllerList[currentAreaIndex].InitArea();
	}

	public void PlayStage()
	{
		OnStageStart?.Invoke();
	}

	public void PauseStage()
	{
	}

	public void EndStage()
	{
		OnStageEnd?.Invoke();
	}

	// Index
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
			FDebug.Log($"[{GetType()}] ���� Active ���°� �ƴմϴ�.");
			FDebug.Break();

			return;
		}
	}

	private void SetRunStatus(bool isOn)
	{
		if (isRunning == isOn)
		{
			return;
		}

		isRunning = isOn;

		OnStageActive?.Invoke(isRunning);
	}
}
