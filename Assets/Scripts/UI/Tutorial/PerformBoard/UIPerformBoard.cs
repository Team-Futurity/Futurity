using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoard : MonoBehaviour
{
	[SerializeField]
	private UIPerformActionData[] actionDatas;

	private Dictionary<PlayerInputEnum, UIPerformActionDataGroup> actionDic;

	private int activeActionCount;
	
	private bool isClear = false;

	// �ִϸ��̼ǿ� ���� �߿��� �����͸� �Է¹��� �ʵ��� �ϰԲ� �ʿ���. 
	private Animator anim;
	
	private void Awake()
	{
		actionDic = new Dictionary<PlayerInputEnum, UIPerformActionDataGroup>();
		
		TryGetComponent(out anim);
		
		for (int i = 0; i < actionDatas.Length; ++i)
		{
			var condition = actionDatas[i].conditionAction;
			var imageObject = transform.GetChild(i).GetComponent<Image>();

			if (imageObject == null)
			{
				return;
			}

			var dataGroup = new UIPerformActionDataGroup(actionDatas[i], imageObject);
			
			actionDic.Add(condition, dataGroup);
			actionDic[condition].SetImage(ActionImageType.NORMAL);
		}

		activeActionCount = actionDic.Count;
	}

	public bool SetPerformAction(PlayerInputEnum data)
	{
		var index = UpdatePerformAction(data);

		isClear = (index <= 0);
		
		return isClear;
	}

	public void SetActive(bool isOn)
	{
		gameObject.SetActive(isOn);
	}

	private int UpdatePerformAction(PlayerInputEnum data)
	{
		if (actionDic[data].GetChecked())
		{
			return activeActionCount;
		}
		
		actionDic[data].SetImage(ActionImageType.CLEAR);
		actionDic[data].SetChecked(true);

		return --activeActionCount;
	}
}
