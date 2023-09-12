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
			FDebug.Log($"[{GetType()}] ControllerObserverList에 아무 것도 존재하지 않습니다.");
			FDebug.Break();
		}

		if(AreaID == 0)
		{
			FDebug.Log($"[{GetType()}] AreaID가 부여되지 않았습니다.");
			FDebug.Break();
		}
	}

	// Area Controller
	// ControlMethod 


}
