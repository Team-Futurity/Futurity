using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	private UIPerformBoard currentBoard;
	// Index
	private int currentIndex;

	private void Awake()
	{
		if(PerformBoardList is null)
		{
			FDebug.LogError($"{PerformBoardList.GetType()}이 존재하지 않습니다.");
			
			Debug.Break();
		}

		currentIndex = 0;
		currentBoard = PerformBoardList[currentIndex];
	}

	public void UpdateAction(PlayerInputEnum data)
	{
	}

	public void CheckAllPass()
	{
		OnNextBoard();
	}

	private void OnNextBoard()
	{
		SetBoard();
	}

	private void OnBeforeBoard()
	{
		SetBoard();
	}

	private void SetBoard()
	{
	}
}
