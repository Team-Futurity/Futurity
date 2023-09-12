using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaController : MonoBehaviour
{
	[field: SerializeField]
	public int AreaID { get; private set; }

	[field: SerializeField]
	public List<IControlCommand> ControlObserverList { get; private set; }

	// Events : Area Start, Area End
	public UnityEvent OnAreaInit;
	public UnityEvent OnAreaStart;
	public UnityEvent OnAreaEnd;

	// Portal
	public Transform startPortalTrans;
	public Transform endPortalTrans;

	private void Awake()
	{
		if (ControlObserverList == null)
		{
			FDebug.Log($"[{GetType()}] ControllerObserverList에 아무 것도 존재하지 않습니다.");
			FDebug.Break();
		}

	}

	public void PlayArea()
	{
		foreach(var observer in ControlObserverList)
		{
			observer.Run();
		}
	}

	public void StopArea()
	{

	}

	public void EndArea()
	{

	}
}
