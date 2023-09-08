using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : Singleton<GameManagerBase>
{
	private List<IControllerMethod> controllObserverList;

	protected override void Awake()
	{
		base.Awake();

		controllObserverList = new List<IControllerMethod>();
	}

	public void RegisterObserver(IControllerMethod observer)
	{
		controllObserverList.Add(observer);
	}

	public void RemoveObserver(IControllerMethod observer)
	{
		var removeObserver = controllObserverList.Find(x =>
		{
			return x.GetHashCode() == observer.GetHashCode();
		});

		controllObserverList.Remove(removeObserver);
	}

	public void RemoveAll()
	{
		controllObserverList.Clear();
	}

	public void ActiveObserver()
	{
		foreach(var observer in controllObserverList)
		{
			observer.Active();
		}
	}

	public void RunObserver()
	{
		foreach (var observer in controllObserverList)
		{
			observer.Run();
		}
	}

	public void StopObserver()
	{
		foreach (var observer in controllObserverList)
		{
			observer.Stop();
		}
	}
}
