using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event를 통해서 Buff의 시작과 끝에서 기획자들이 행동할 수 있도록 정의함.

	[field: Header("Data")]
	[field: SerializeField] public BuffData BuffData { get; private set; }
	public BuffName CurrBuffName = BuffName.NONE;

	[Space(10)]
	[Header("Event")]
	public UnityEvent buffStart;
	public UnityEvent buffEnd;
	public UnityEvent buffStay;

	private float buffActiveTime;
	private float currTime;
	protected UnitBase targetUnit = null;

	private void Start()
	{
		if (CurrBuffName == BuffName.NONE)
		{
			FDebug.Log("Curr Buf의 Name이 정해지지 않았습니다.");
			Debug.Break();
		}

		if (targetUnit == null)
		{
			FDebug.Log("TargetUnit이 NULL 입니다.");
			Debug.Break();
		}

		if (BuffData == null)
		{
			FDebug.Log("Buff Data가 존재하지 않습니다.");
			Debug.Break();
		}
		
		buffActiveTime = BuffData.BuffActiveTime;
		currTime = .0f;
	}

	private void Update()
	{
		buffActiveTime -= Time.deltaTime;
		currTime += Time.deltaTime;

		if (1 < currTime)
		{
			currTime = .0f;
			buffStay?.Invoke();
		}

		if(0 > buffActiveTime)
		{
			UnActive();
		}
	}

	public virtual void Active(UnitBase unit)
	{
		buffStart?.Invoke();

		targetUnit = unit;
		
		FDebug.Log($"{CurrBuffName}가 실행되었습니다.");
	}

	public virtual void UnActive()
	{
		buffEnd?.Invoke();
		
		FDebug.Log($"{CurrBuffName}가 종료되었습니다.");
		Destroy(gameObject);
	}
}
