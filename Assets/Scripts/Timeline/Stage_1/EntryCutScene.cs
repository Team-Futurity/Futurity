using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntryCutScene : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private GameObject uiCanvas;
	[SerializeField] private GameObject playerCamera;

	[Header("진입 컷신에서 활성화할 오브젝트 목록")]
	[SerializeField] private GameObject[] walls;

	[Header("Event")] 
	[SerializeField] private UnityEvent entryCutSceneEndEvent;

	private GameObject player = null;
	private UnityEngine.InputSystem.PlayerInput playerInput;
	
	private void Start()
	{
		//컷신을 재생하는 함수가 다른곳에서 불릴 때까지 해당 지점에서 1구역 진입 연출을 시작합니다.
		PlayEntryCutScene();
	}
	
	private void PlayEntryCutScene()
	{
		//uiCanvas.SetActive(false);
		
		if (player is null)
		{
			player = GameObject.FindWithTag("Player");
		}
		
		playerInput = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		playerInput.enabled = false;
		Time.timeScale = 0.0f;
	}

	public void EndEntryCutScene()
	{
		foreach (var wall in walls)
		{
			wall.SetActive(true);
		}
		uiCanvas.SetActive(true);
		playerInput.enabled = true;
		entryCutSceneEndEvent.Invoke();
		
		Player statusPlayer = player.GetComponent<Player>();
		statusPlayer.hpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(
			statusPlayer.status.GetStatus(StatusType.CURRENT_HP).GetValue() /
			statusPlayer.status.GetStatus(StatusType.MAX_HP).GetValue());
		
		playerCamera.SetActive(true);
		gameObject.SetActive(false);
	}
	
	public void EnableEnemy()
	{
		Time.timeScale = 1.0f;
	}
}
