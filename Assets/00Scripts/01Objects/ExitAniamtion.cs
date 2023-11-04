using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitAniamtion : MonoBehaviour
{
	[Header("Component")] 
	private ChapterMoveController chapterMoveController;
	[SerializeField] private SkeletonAnimation doorAnimation;
	[SerializeField] private float delayTime = 1.5f;

	[Header("조건")] 
	[SerializeField] private int enableCondition;
	[SerializeField, ReadOnly(false)] private int curCount;

	public void PlusCurCount()
	{
		curCount++;

		if (curCount < enableCondition)
		{
			return;
		}

		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
	
	private void Start()
	{
		chapterMoveController = ChapterMoveController.Instance;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		chapterMoveController.EnableInteractionUI(EUIType.NEXTSTAGE);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		chapterMoveController.DisableInteractionUI(EUIType.NEXTSTAGE);
	}
	
	private void CheckMoveStage(InputAction.CallbackContext context)
	{
		StartCoroutine(WaitDoorOpen());
	}

	private IEnumerator WaitDoorOpen()
	{
		doorAnimation.AnimationState.SetAnimation(0, "open", false);

		yield return new WaitForSeconds(delayTime);
		
		chapterMoveController.MoveNextChapter();
		chapterMoveController.DisableInteractionUI(EUIType.NEXTSTAGE);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		gameObject.SetActive(false);
	}
}
