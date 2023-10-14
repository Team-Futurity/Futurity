using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ExitCutScene : CutSceneBase
{
	[Header("수치값")]
	[SerializeField] private float moveDistance = 7.0f;
	[SerializeField] private float duration = 0.0f;
	[SerializeField] private SpawnerManager enemySpawner;

	private Vector3 originUIPos = Vector3.zero;
	
	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		chapterManager.ChangeFollowTarget(true, null);
		chapterManager.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		chapterManager.SetActivePlayerInput(true);
	}
	
	public void MovePlayer()
	{
		Transform playerTf = GameObject.FindWithTag("Player").transform;
		var targetRot = Quaternion.LookRotation(Vector3.zero);
		playerTf.localRotation = targetRot;
		
		var targetPos = chapterManager.GetTargetPosition(moveDistance, Vector3.forward);
		chapterManager.PlayerController.LerpToWorldPosition(targetPos, duration);
	}

	public void ResetCameraTarget()
	{
		chapterManager.ChangeFollowTarget();	
	}
	
	public void SpawnEnemy()
	{
		enemySpawner.SpawnEnemy();
	}
}
