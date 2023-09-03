using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	private void Awake()
	{
		if(PerformBoardList is null)
		{
			FDebug.LogError($"{PerformBoardList.GetType()}�� �������� �ʽ��ϴ�.");
			
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
