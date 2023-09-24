using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour
{
	// Data를 전달받는 타겟 오브젝트
	[field: Header("플레이어 데이터")]
	
	[field: SerializeField] 
	public PlayerInputManager Target { get; private set; }

	[field: Header("Board 목록")] 
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
			FDebug.LogError($"{PerformBoardList.GetType()}이 존재하지 않습니다.");
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
