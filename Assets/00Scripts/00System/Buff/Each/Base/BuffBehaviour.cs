using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event를 통해서 Buff의 시작과 끝에서 기획자들이 행동할 수 있도록 정의함.
	[field: Header("Data")]
	[field: SerializeField] public BuffData BuffData { get; private set; }

	[Space(10)]
	[Header("Event")]
	public UnityEvent buffStart;
	public UnityEvent buffEnd;
	public UnityEvent buffStay;

	private float buffActiveTime;
	private float currTime;

	
	protected UnitBase targetUnit = null;
	protected BuffSystem targetBuff = null;

	private void Start()
	{
		if (BuffData.BuffName == BuffName.NONE)
		{
			FDebug.Log("Curr Buf의 Name이 정해지지 않았습니다.");
			Debug.Break();
		}

		if (BuffData == null)
		{
			FDebug.Log("Buff Data가 존재하지 않습니다.");
			Debug.Break();
		}

		currTime = .0f;
	}

	private void Update()
	{
		buffActiveTime -= Time.deltaTime;
		currTime += Time.deltaTime;

		if (1 <= currTime)
		{
			currTime = .0f;
			buffStay?.Invoke();
		}

		if(0 >= buffActiveTime)
		{
			UnActive();
		}
	}

	public void Create(UnitBase unit, float activityTime = -1f)
	{
		gameObject.SetActive(false);

		targetUnit = unit;
		unit.TryGetComponent(out targetBuff);

		transform.parent = unit.transform;

		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;

		gameObject.SetActive(true);

		SetBuffTime(activityTime);

		Active();
	}

	public void SetBuffTime(float activityTime = -1f)
	{
		if (activityTime is -1f)
		{
			buffActiveTime = BuffData.BuffActiveTime;
			return;
		}

		buffActiveTime = activityTime;
	}

	public virtual void Active()
	{
		buffStart?.Invoke();
		targetBuff.AddBuff(this);

		FDebug.Log($"{BuffData.BuffName}가 실행되었습니다.");
	}

	public virtual void UnActive()
	{
		targetBuff.RemoveBuff(BuffData.BuffCode);

		buffEnd?.Invoke();
		
		FDebug.Log($"{BuffData.BuffName}가 종료되었습니다.");

		Destroy(gameObject);
	}
}
