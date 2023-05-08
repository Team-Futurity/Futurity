using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event를 통해서 Buff의 시작과 끝에서 기획자들이 행동할 수 있도록 정의함.

	[field: Header("Data")]
	[field: SerializeField] public BuffData BuffData { get; private set; }
	[field: SerializeField] public BuffName CurrBuffName = BuffName.NONE;

	[Space(10)]
	[Header("Event")]
	public UnityEvent buffStart;

	public UnityEvent buffEnd;


	private float buffActiveTime = .0f;

	private void Start()
	{
		if (CurrBuffName == BuffName.NONE)
		{
			FDebug.Log("Curr Buf의 Name이 정해지지 않았습니다.");
		}
	}

	private void Update()
	{
		buffActiveTime -= Time.deltaTime;

		if (0 >= buffActiveTime)
		{
			UnActive();
		}
	}

	public virtual void Active(UnitBase unit, float activeTime)
	{
		buffActiveTime = activeTime;
		buffStart.Invoke();
		
		FDebug.Log($"{CurrBuffName}가 실행되었습니다.");
	}

	public virtual void UnActive()
	{
		buffEnd.Invoke();
		
		FDebug.Log($"{CurrBuffName}가 종료되었습니다.");
		Destroy(gameObject);
	}
}
