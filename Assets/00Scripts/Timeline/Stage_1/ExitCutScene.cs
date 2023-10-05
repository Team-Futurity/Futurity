using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ExitCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private PlayableDirector exitTimeline;
	[SerializeField] private DialogData dialogData;
	
	[Header("수치값")]
	[SerializeField] private float moveDistance = 7.0f;
	[SerializeField] private float duration = 0.0f;
	[SerializeField] private SpawnerManager enemySpawner;
	
	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		TimelineManager.Instance.uiCanvas.SetActive(false);
		TimelineManager.Instance.ChangeFollowTarget(true, null);
		TimelineManager.Instance.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		
	}
	
	public void MovePlayer()
	{
		Transform playerTf = GameObject.FindWithTag("Player").transform;
		var targetRot = Quaternion.LookRotation(Vector3.zero);
		playerTf.localRotation = targetRot;
		
		var targetPos = TimelineManager.Instance.GetOffsetVector(moveDistance, Vector3.forward);
		TimelineManager.Instance.PlayerController.LerpToWorldPosition(targetPos, duration);
	}

	public void SpawnEnemy()
	{
		enemySpawner.SpawnEnemy();
	}

	public void PlayDialogExitCutScene()
	{
		exitTimeline.Pause();
		TimelineManager.Instance.StartDialog(dialogData);
		
		TimelineManager.Instance.DialogController.OnEnded?.AddListener( () =>
		{
			exitTimeline.Resume();	
		});
	}
}
