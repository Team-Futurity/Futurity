using UnityEngine;
using UnityEngine.InputSystem;

public class ChapterMoveTrigger : MonoBehaviour
{
	[Header("Component")] 
	private ChapterMoveController chapterMoveController;
	private bool isInput = false;

	private void Start()
	{
		chapterMoveController = gameObject.GetComponentInParent<ChapterMoveController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		chapterMoveController.SetActiveInteractionUI(true);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}

		if (isInput == false)
		{
			return;
		}
		
		chapterMoveController.MoveNextChapter();
		chapterMoveController.SetActiveInteractionUI(false);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		gameObject.SetActive(false);
	}

	private void CheckMoveStage(InputAction.CallbackContext context)
	{
		isInput = true;
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		chapterMoveController.SetActiveInteractionUI(false);
	}
}
