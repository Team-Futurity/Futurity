using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpener : MonoBehaviour
{
	[field: Space(10)]
	[field: SerializeField]
	public UIWindow window { get; private set; }

	[field: SerializeField]
	public bool usedAwakeOpen { get; private set; }

	private void Awake()
	{
		if(window == null)
		{
			FDebug.Log($"[{GetType()}] 현재 Window가 할당되지 않았습니다.");
			FDebug.Break();

			return;
		}

		if(usedAwakeOpen)
		{
			OnWindow();
		}
	}

	public void OnWindow()
	{
		if(!window.gameObject.activeSelf)
		{
			window.gameObject.SetActive(true);
		}
	}
}
