using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCutScene : CutSceneBase
{
	[SerializeField] private float moveDistance = 7.0f;
	[SerializeField] private float duration = 0.0f;
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
}
