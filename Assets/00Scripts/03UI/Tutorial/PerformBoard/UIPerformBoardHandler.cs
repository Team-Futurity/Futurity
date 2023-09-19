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
	private UIPerformBoard currentBoard;
	
	// Index
	private int currentIndex;
	private int maxIndex;
	
	private bool isActive;

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

		isActive = false;
	}

	public void SetPerfrom()
	{
		isActive = true;

		currentIndex = 0;
		maxIndex = PerformBoardList.Count - 1;
		
		currentBoard = PerformBoardList[currentIndex];
		currentBoard.SetActive(true);
	}

	public void Run()
	{
		Target.onChangeStateEvent?.AddListener(UpdateAction);
	}

	public void Stop()
	{
		Target.onChangeStateEvent?.RemoveListener(UpdateAction);
	}

	private void UpdateAction(PlayerInputEnum data)
	{
		var isComplate = currentBoard.SetPerformAction(data);
		
		if (!isComplate)
		{
			return;
		}

		currentBoard.SetActive(false);

		OnChangePerformBoard?.Invoke();
		Stop();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			currentBoard.SetActive(false);

			OnChangePerformBoard?.Invoke();
			Stop();
		}
	}

	public void ChangeToNextBoard()
	{
		Run();

		if (currentIndex >= maxIndex)
		{
			GetEndProcess();
			
			return;
		}
		
		currentIndex++;
		
		currentBoard = PerformBoardList[currentIndex];
		currentBoard.SetActive(true);
	}

	private void GetEndProcess()
	{
		OnEnded?.Invoke();
	}
}
