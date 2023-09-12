using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// ���� Stage�� ���������� Ȯ���ϱ�
	private bool isActive = false;

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
			FDebug.Log($"[{GetType()}] ���� Active ���°� �ƴմϴ�.");
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
