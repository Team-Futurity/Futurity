using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Event�� ���ؼ� Buff�� ���۰� ������ ��ȹ�ڵ��� �ൿ�� �� �ֵ��� ������.
	public UnityEvent buffStart;
	public UnityEvent buffEnd;
	
	public BuffName currBuffName = BuffName.NONE;

	private float buffActiveTime = .0f;

	private void Start()
	{
		if (currBuffName == BuffName.NONE)
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
		
		FDebug.Log($"{currBuffName}�� ����Ǿ����ϴ�.");
	}

	public virtual void UnActive()
	{
		buffEnd.Invoke();
		
		FDebug.Log($"{currBuffName}�� ����Ǿ����ϴ�.");
		Destroy(gameObject);
	}
}
