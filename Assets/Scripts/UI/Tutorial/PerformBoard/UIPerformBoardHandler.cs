using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour, IControlCommand
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
	private int maxIndex;
	
	private bool isActive;

	[HideInInspector] 
	public UnityEvent OnChangePerformBoard;
	
	private void Awake()
	{
		if(PerformBoardList is null)
		{
			FDebug.LogError($"{PerformBoardList.GetType()}�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}

		foreach (var board in PerformBoardList)
		{
			board.SetActive(false);
		}

		isActive = false;
	}

	#region IControlCommand
	void IControlCommand.Init()
	{
		isActive = true;

		currentIndex = 0;
		maxIndex = PerformBoardList.Count - 1;
		
		currentBoard = PerformBoardList[currentIndex];
		currentBoard.SetActive(true);
		
		Target.onChangeStateEvent?.AddListener(UpdateAction);
	}

	void IControlCommand.Run()
	{
		Target.onChangeStateEvent?.AddListener(UpdateAction);
	}

	void IControlCommand.Stop()
	{
		Target.onChangeStateEvent?.RemoveListener(UpdateAction);
	}

	#endregion

	private void UpdateAction(PlayerInputEnum data)
	{
		var isComplate = currentBoard.SetPerformAction(data);
		
		if (!isComplate)
		{
			return;
		}
		
		OnChangePerformBoard?.Invoke();
		
		ChangeToNextBoard();
	}

	private void ChangeToNextBoard()
	{
		if (currentIndex >= maxIndex)
		{
			GetEndProcess();
			
			return;
		}
		currentBoard.SetActive(false);
		
		currentIndex++;
		
		currentBoard = PerformBoardList[currentIndex];
		currentBoard.SetActive(true);
	}

	private void GetEndProcess()
	{
		Debug.Log("Perform Board Action Clear");
	}
}
