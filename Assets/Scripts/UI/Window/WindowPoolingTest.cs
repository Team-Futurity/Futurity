using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPoolingTest : MonoBehaviour
{
	public GameObject dummyPrefab;
	public GameObject dummyParent;
	[SerializeField]
	private List<ObjectPoolManager<WindowController>> poolingWindows = new List<ObjectPoolManager<WindowController>>();
	private void Start()
	{
		for(int i = 0; i < 5; i++)
		{
			poolingWindows.Add(new ObjectPoolManager<WindowController>(dummyPrefab, dummyParent));
			poolingWindows[i].ActiveObject();
			WindowController obj = poolingWindows[i].ActiveObject();
			poolingWindows[i].DeactiveObject(obj);
			poolingWindows[i].ActiveObject();
		}
	}
}
