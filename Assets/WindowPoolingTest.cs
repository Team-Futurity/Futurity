using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPoolingTest : MonoBehaviour
{
	public GameObject dummyPrefab;
	public GameObject dummyParent;
	[SerializeField]
	private List<ObjectPoolManager<FloatingText>> poolingWindows = new List<ObjectPoolManager<FloatingText>>();


	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			poolingWindows.Add(new ObjectPoolManager<FloatingText>(dummyPrefab, dummyParent));
			poolingWindows[i].ActiveObject();
			FloatingText obj = poolingWindows[i].ActiveObject();
			poolingWindows[i].DeactiveObject(obj);
			poolingWindows[i].ActiveObject();
		}
	}
}
