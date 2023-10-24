using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour
{
	// Data를 전달받는 타겟 오브젝트
	[SerializeField, Header("플레이어 데이터")]
	private PlayerInputManager target;

	[SerializeField]
	private List<UIPerformBoard> performBoards;

	private UIPerformBoard currentBoard;

	private void Awake()
	{
		currentBoard = Pop();
		currentBoard.Active(true);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			GetTargetInputData(PlayerInputEnum.Move);
		}
	}

	private void GetTargetInputData(PlayerInputEnum type)
	{
		var isClear = currentBoard.EnterPlayerEventType(type);

		if (isClear)
		{
			ChangeBoard();	
		}
	}

	private void ChangeBoard()
	{
		currentBoard.Active(false);
		
		currentBoard = Pop();

		if (currentBoard == null)
		{
			Debug.Log("모든 작업이 종료되었습니다.");
			return;
		}
		
		currentBoard.Active(true);
	}

	private UIPerformBoard Pop()
	{
		if (performBoards.Count <= 0)
		{
			return null;
		}
		
		var data = performBoards[0];
		performBoards.RemoveAt(0);
		
		return data;
	}
}
