using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour, IControllerMethod
{
	// Data�� ���޹޴� Ÿ�� ������Ʈ
	[field: Header("�÷��̾� ������")]
	
	[field: SerializeField] 
	public PlayerInputManager Target { get; private set; }

	[field: Header("Board ���")] 
	[field: SerializeField]
	public List<UIPerformBoard> PerformBoardList { get; private set; }

	// Current Board
	private UIPerformBoard currentBoard;
	// Index
	private int currentIndex;

	private bool isActive;

	private void Awake()
	{
		if(PerformBoardList is null)
		{
			FDebug.LogError($"{PerformBoardList.GetType()}�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}

		isActive = false;
		currentIndex = 0;
		currentBoard = PerformBoardList[currentIndex];
	}

	#region IControllerMethod
	void IControllerMethod.Active()
	{
		isActive = true;
		
		Target?.onChangeStateEvent.AddListener(UpdateAction);
	}

	void IControllerMethod.Run()
	{
		Target?.onChangeStateEvent.AddListener(UpdateAction);
	}

	void IControllerMethod.Stop()
	{
		Target?.onChangeStateEvent.RemoveListener(UpdateAction);
	}

	#endregion
	
	private void UpdateAction(PlayerInputEnum data)
	{
		if (!isActive)
		{
			return;
		}
		
		var isComplate = currentBoard.SetPerformAction(data);
	}

	private void SetNextBoard()
	{
		
	}
	
}
