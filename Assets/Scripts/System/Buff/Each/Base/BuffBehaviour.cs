using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event�� ���ؼ� Buff�� ���۰� ������ ��ȹ�ڵ��� �ൿ�� �� �ֵ��� ������.
	[field: Header("Data")]
	[field: SerializeField] public BuffData BuffData { get; private set; }

	[Space(10)]
	[Header("Event")]
	public UnityEvent buffStart;
	public UnityEvent buffEnd;
	public UnityEvent buffStay;

	[HideInInspector] public BuffSystem executor;

	private float buffActiveTime;
	private float currTime;
	
	protected UnitBase targetUnit = null;

	private void Start()
	{
		if (BuffData.BuffName == BuffNameList.NONE)
		{
			FDebug.Log("Curr Buf�� Name�� �������� �ʾҽ��ϴ�.");
			Debug.Break();
		}

		if (BuffData == null)
		{
			FDebug.Log("Buff Data�� �������� �ʽ��ϴ�.");
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

	public void SetExecutor(BuffSystem sendExecutor)
	{
		if(sendExecutor is null)
		{
			FDebug.Log("[BuffSystem] �߸��� �����Դϴ�.");
			return;
		}

		executor = sendExecutor;
	}

	public virtual void Active(UnitBase unit)
	{
		buffStart?.Invoke();

		targetUnit = unit;
		
		FDebug.Log($"{BuffData.BuffName}�� ����Ǿ����ϴ�.");
	}

	public virtual void UnActive()
	{
		buffEnd?.Invoke();
		
		FDebug.Log($"{BuffData.BuffName}�� ����Ǿ����ϴ�.");

		executor.RemoveActiveBuff(BuffData.BuffCode);

		Destroy(gameObject);
	}
}
