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
	
	private void Awake()
	{
		actionDic = new Dictionary<PlayerInputEnum, UIPerformActionDataGroup>();
		
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

	// Test Code

	private PlayerInputEnum[] testInputData =
	{
		PlayerInputEnum.Move, PlayerInputEnum.Dash, PlayerInputEnum.NormalAttack_J,
		PlayerInputEnum.NormalAttack_JJ, PlayerInputEnum.NormalAttack_JJJ
	};
	
	private int testIndex = 0;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetPerformAction(testInputData[testIndex++]);
		}
	}
	
	public bool SetPerformAction(PlayerInputEnum data)
	{
		var index = UpdatePerformAction(data);
		
		if (index <= 0)
		{
			Debug.Log("CLEAR!");
			
			return isClear;
		}

		return isClear;
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
