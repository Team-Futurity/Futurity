using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaController : MonoBehaviour
{
	[field: SerializeField]
	public int AreaID { get; private set; }

	// Events : Area Start, Area End
	public UnityEvent OnAreaInit;
	public UnityEvent OnAreaStart;
	public UnityEvent OnAreaEnd;

	// Controller Method

	// Portal
	public Transform startPortalTrans;
	public Transform endPortalTrans;

	private void Awake()
	{
		//if(controllerObserverList == null)
		{
			FDebug.Log($"[{GetType()}] ControllerObserverList�� �ƹ� �͵� �������� �ʽ��ϴ�.");
			FDebug.Break();
		}

		if(AreaID == 0)
		{
			FDebug.Log($"[{GetType()}] AreaID�� �ο����� �ʾҽ��ϴ�.");
			FDebug.Break();
		}
	}

	// Area Controller
	// ControlMethod 


}
