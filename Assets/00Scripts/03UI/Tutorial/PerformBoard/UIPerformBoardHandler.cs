using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour
{
	// Data�� ���޹޴� Ÿ�� ������Ʈ
	[field: Header("�÷��̾� ������")]
	
	[field: SerializeField] 
	public PlayerInputManager Target { get; private set; }

	[field: Header("Board ���")] 
	[field: SerializeField]
	public List<UIPerformBoard> PerformBoardList { get; private set; }

	// Current Board
	[SerializeField]
	private UIPerformBoard currentBoard;
	
	// Index
	private int currentIndex;
	private int maxIndex;
	
	[HideInInspector] 
	public UnityEvent OnChangePerformBoard;

	[HideInInspector]
	public UnityEvent OnEnded;
	
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
		
		currentIndex = 0;
		maxIndex = PerformBoardList.Count - 1;
	}

	public void SetPerfrom()
	{
		currentBoard = PerformBoardList[currentIndex];
		currentBoard.SetActive(true);
	}

	public void Run()
	{
		Target.onChangeStateEvent?.AddListener(UpdateAction);
	}

	public void Stop()
	{
		currentBoard.SetActive(false);
		Target.onChangeStateEvent?.RemoveListener(UpdateAction);
	}

	private void UpdateAction(PlayerInputEnum data)
	{
		var isComplate = currentBoard.SetPerformAction(data);
		
		if (!isComplate)
		{
			return;
		}
		
		Stop();
		
		if (currentIndex >= maxIndex)
		{
			OnEnded?.Invoke();
			return;
		}
		else
		{
			OnChangePerformBoard?.Invoke();
		}
	}

	public void ChangeToNextBoard()
	{
		Run();
		
		currentIndex++;
		SetPerfrom();
	}
}
