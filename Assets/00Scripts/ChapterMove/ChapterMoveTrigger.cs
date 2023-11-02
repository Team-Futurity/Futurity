using UnityEngine;
using UnityEngine.InputSystem;

public class ChapterMoveTrigger : MonoBehaviour
{
	[Header("Component")] 
	private ChapterMoveController chapterMoveController;
	private bool isInput = false;

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
		chapterMoveController.MoveNextChapter();
		chapterMoveController.EnableInteractionUI(EUIType.NEXTSTAGE);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckMoveStage);
		gameObject.SetActive(false);
	}

}
