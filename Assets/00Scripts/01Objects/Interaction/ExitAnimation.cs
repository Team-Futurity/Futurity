using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ExitAnimation : MonoBehaviour
{
	[Header("컷 씬 사용 여부")]
	[SerializeField] private bool isUseCutScene;
	[SerializeField] private ECutSceneType cutSceneType;
	
	[Header("Component")]
	[SerializeField] private SkeletonAnimation doorAnimation;
	[SerializeField] private float delayTime = 1.5f;
	private ChapterMoveController chapterMoveController;

	[Header("조건")] 
	[SerializeField] private int enableCondition;
	[SerializeField, ReadOnly(false)] private int curCount;

	private ObjectIndicator objectIndicator;
	private IEnumerator doorOpen;

	public void PlusCurCount()
	{
		curCount++;

		if (curCount < enableCondition)
		{
			return;
		}

		gameObject.GetComponent<BoxCollider>().enabled = true;
		Invoke(nameof(ActiveIndicator), 1.8f);
	}

	public void DoorOpenWait(UnityAction action = null)
	{
		doorOpen = DoorOpen(action);
		StartCoroutine(doorOpen);
	}
	
	private void Start()
	{
		chapterMoveController = ChapterMoveController.Instance;
		objectIndicator = GameObject.FindWithTag("Player").GetComponentInChildren<ObjectIndicator>();
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
		if (isUseCutScene == true)
		{
			objectIndicator.DeactiveIndicator();
			
			chapterMoveController.DisableInteractionUI(EUIType.NEXTSTAGE);
			InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
			TimelineManager.Instance.EnableCutScene(cutSceneType);

			return;
		}
		
		StartCoroutine(WaitDoorOpen());
	}

	private IEnumerator WaitDoorOpen()
	{
		doorAnimation.AnimationState.SetAnimation(0, "open", false);
		objectIndicator.DeactiveIndicator();

		yield return new WaitForSeconds(delayTime);
		
		chapterMoveController.MoveNextChapter();
		chapterMoveController.DisableInteractionUI(EUIType.NEXTSTAGE);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		gameObject.SetActive(false);
	}

	private IEnumerator DoorOpen(UnityAction action)
	{
		InputActionManager.Instance.DisableActionMap();
		doorAnimation.AnimationState.SetAnimation(0, "open", false);
		
		yield return new WaitForSeconds(delayTime);
		
		action?.Invoke();
	}
	
	private void ActiveIndicator()
	{
		objectIndicator.ActivateIndicator(gameObject);
	}
}
