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

	private void Start()
	{
		if (currBuffName == BuffName.NONE)
		{
			FDebug.Log("Curr Buf�� Name�� �������� �ʾҽ��ϴ�.");
		}
	}

	public virtual void Active(UnitBase unit)
	{
		buffStart.Invoke();
		
		FDebug.Log($"{currBuffName}�� ����Ǿ����ϴ�.");
	}

	public virtual void UnActive()
	{
		buffEnd.Invoke();
		
		FDebug.Log($"{currBuffName}�� ����Ǿ����ϴ�.");
	}
}
