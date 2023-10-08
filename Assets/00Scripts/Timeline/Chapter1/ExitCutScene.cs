using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ExitCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private PlayableDirector exitTimeline;
	[SerializeField] private Vector2 dialogUIOffset;
	
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
		TimelineManager.Instance.SetActiveMainUI(false);
		TimelineManager.Instance.ChangeFollowTarget(true, null);
		TimelineManager.Instance.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		TimelineManager.Instance.SetActiveMainUI(true);
		TimelineManager.Instance.SetActivePlayerInput(true);
	}
	
	public void MovePlayer()
	{
		Transform playerTf = GameObject.FindWithTag("Player").transform;
		var targetRot = Quaternion.LookRotation(Vector3.zero);
		playerTf.localRotation = targetRot;
		
		var targetPos = TimelineManager.Instance.GetTargetPosition(moveDistance, Vector3.forward);
		TimelineManager.Instance.PlayerController.LerpToWorldPosition(targetPos, duration);
	}

	public void ResetCameraTarget()
	{
		TimelineManager.Instance.ChangeFollowTarget();	
	}
	
	public void SpawnEnemy()
	{
		enemySpawner.SpawnEnemy();
	}
}
