using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event�� ���ؼ� Buff�� ���۰� ������ ��ȹ�ڵ��� �ൿ�� �� �ֵ��� ������.

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
			FDebug.Log("Curr Buf�� Name�� �������� �ʾҽ��ϴ�.");
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
		
		FDebug.Log($"{CurrBuffName}�� ����Ǿ����ϴ�.");
	}

	public virtual void UnActive()
	{
		buffEnd.Invoke();
		
		FDebug.Log($"{CurrBuffName}�� ����Ǿ����ϴ�.");
		Destroy(gameObject);
	}
}
