using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoard : MonoBehaviour
{
	[SerializeField]
	private UIPerformActionData[] actionDatas;
	private Image[] viewers;

	private const int ARRAY_MAX = 5;
	private int DATA_MAX = 0;

	[HideInInspector]
	public UnityEvent onLastClearEvent;

	private void Awake()
	{
		DATA_MAX = actionDatas.Length - 1;
		viewers = new Image[ARRAY_MAX];

		for (int i = 0; i < ARRAY_MAX; ++i)
		{
			transform.GetChild(i).TryGetComponent(out viewers[i]);

			if (i > DATA_MAX)
			{
				viewers[i].gameObject.SetActive(false);
				return;
			}

			viewers[i].sprite = actionDatas[i].enableSpr;
		}
	}
	public void CheckedAction(PlayerInputEnum data)
	{
		var getIndex = FindIndex(data);

		if (getIndex == -1)
		{
			return;
		}

		viewers[getIndex].sprite = actionDatas[getIndex].disableSpr;
		actionDatas[getIndex].isClear = true;

		CheckClearCount();
	}

	private int FindIndex(PlayerInputEnum data)
	{
		for (int i = 0; i <= DATA_MAX; ++i)
		{
			if (actionDatas[i].clearActionString != data || actionDatas[i].isClear)
			{
				continue;
			}
			else
			{
				return i;
			}
		}

		return -1;
	}

	private void CheckClearCount()
	{
		int count = 0;

		for (int i = 0; i <= DATA_MAX; ++i)
		{
			if (!actionDatas[i].isClear)
				continue;

			count++;
		}

		if (count > DATA_MAX)
		{
			onLastClearEvent?.Invoke();
		}
	}
}
