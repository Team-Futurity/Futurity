using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CrowdBase : MonoBehaviour
{
	// Event를 통해서 Buff의 시작과 끝에서 기획자들이 행동할 수 있도록 정의함.
	[field:SerializeField]
	public CrowdData data { get; private set; }

	[Space(10)]
	[Header("Event")]
	public UnityEvent buffStart;
	public UnityEvent buffEnd;

	private float activeTime = .0f;
	private float currTime;
	
	protected CrowdSystem targetSystem = null;
	protected UnitBase targetUnit = null;

	private bool isStart = false;

	private void Awake()
	{
		currTime = .0f;
		activeTime = data.CrowdActiveTime;
	}

	private void OnDisable()
	{
		isStart = false;
	}

	private void Update()
	{
		if (!isStart) return;
		
		// Timer 진행
		currTime += Time.deltaTime;

		if (currTime > activeTime)
		{
			UnActive();
		}
	}

	public void SetData(CrowdSystem crowdSystem, UnitBase unit)
	{
		targetSystem = crowdSystem;
		targetUnit = unit;
		
		Active();
	}

	private void Active()
	{
		if (isStart) return;
		
		buffStart?.Invoke();
		
		isStart = true;
		StartCrowd();
	}

	public void UnActive()
	{
		ExitCrowd();

		targetSystem.RemoveCrowdData(this);
		buffEnd?.Invoke();
	}

	public void SetCrowdTime(float time)
	{
		activeTime = time;
	}

	protected abstract void StartCrowd();
	protected abstract void ExitCrowd();
}
