using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaController : MonoBehaviour
{
	[field: SerializeField]
	public int AreaID { get; private set; }

	// Events : Area Start, Area End
	public UnityEvent<bool> OnAreaStart;
	public UnityEvent<bool> OnAreaEnd;

	// Controller Method
	public List<IControllerMethod> controllerObserverList;

	// Portal
	public Transform startPortalTrans;
	public Transform endPortalTrans;

	private void Awake()
	{
		if(controllerObserverList == null)
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

	private void Start()
	{
		InitAll();
	}

	private void Update()
	{

	}

	private void OnDrawGizmos()
	{
		if (startPortalTrans == null || endPortalTrans == null)
		{
			return;
		}

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(startPortalTrans.position, 1f);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(endPortalTrans.position, 1f);
	}

	public void InitAll()
	{
		foreach (var observer in controllerObserverList)
		{
			observer.Init();
		}
	}

	public void RunAll()
	{
		foreach (var observer in controllerObserverList)
		{
			observer.Run();
		}
	}

	public void StopAll()
	{
		foreach (var observer in controllerObserverList)
		{
			observer.Stop();
		}
	}


}
