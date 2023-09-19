using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpener : MonoBehaviour
{
	[field: Space(10)]
	[field: Header("������ ������")]
	[field: SerializeField]
	public UIWindow window { get; private set; }

	[field: Space(10)]
	[field: SerializeField]
	public bool usedAwakeOpen { get; private set; }

	private void Awake()
	{
		if(window == null)
		{
			FDebug.Log($"[{GetType()}] ���� Window�� �Ҵ���� �ʾҽ��ϴ�.");
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
		if(!window.gameObject.activeSelf && window.CurrentState == WindowState.NONE)
		{
			window.gameObject.SetActive(true);
		}
	}
}
