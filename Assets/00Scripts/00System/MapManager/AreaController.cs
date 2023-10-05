using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaController : MonoBehaviour
{
	[field: SerializeField]
	public bool UseDirecting { get; private set; }
	public ECutScene directingType;
	
	[field:Space(10)]

	[field: SerializeField]
	public List<GameObject> ControlObserverList { get; private set; }

	// Portal
	public Transform startPortalTrans;
	public Transform endPortalTrans;

	private bool isActive = false;
	
	[Space(10)]
	[Header("Events")]
	[Space(5)]
	public UnityEvent OnAreaClear;
	
	private void Awake()
	{
		if (ControlObserverList == null)
		{
			FDebug.Log($"[{GetType()}] ControllerObserverList에 아무 것도 존재하지 않습니다.");
			FDebug.Break();
		}
	}
	
	public void InitArea()
	{
		if(isActive)
		{
			return;
		}

		isActive = true;

		foreach(var observer in ControlObserverList)
		{
			observer.TryGetComponent<IControlCommand>(out var command);
			command.Init();
		}

		if(UseDirecting && TimelineManager.Instance.IsDebugMode == false)
		{
			TimelineManager.Instance.EnableCutScene(directingType);
		}
	}

	public void PlayArea()
	{
		foreach(var observer in ControlObserverList)
		{
			observer.TryGetComponent<IControlCommand>(out var command);
			command.Run();
		}
	}

	public void StopArea()
	{
		foreach(var observer in ControlObserverList)
		{
			observer.TryGetComponent<IControlCommand>(out var command);
			command.Stop();
		}
	}

	public void EndArea()
	{
		if(!isActive)
		{
			return;
		}

		isActive = false;
		StopArea();

		OnAreaClear?.Invoke();
	}
}
