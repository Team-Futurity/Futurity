using UnityEngine;

public class ChapterMoveTrigger : MonoBehaviour
{
	[Header("Component")] 
	private ChapterMoveController chapterMoveController;

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
		
		chapterMoveController.SetActiveInteractionUI(true);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			switch (chapterMoveController.CurrentChapter)
			{
				case EChapterType.NONEVENTCHAPTER:
					break;
					
				case EChapterType.CHAPTER1_1:
					chapterMoveController.MoveNextChapter();
					break;
				
				case EChapterType.CHAPTER1_2:
					break;
				
				case EChapterType.CHAPTER_BOSS:
					break;
				
				default:
					return;
			}
			
			chapterMoveController.SetActiveInteractionUI(false);
			gameObject.SetActive(false);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		chapterMoveController.SetActiveInteractionUI(false);
	}
}
