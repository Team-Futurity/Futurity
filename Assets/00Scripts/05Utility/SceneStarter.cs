using UnityEngine;

public class SceneStarter : MonoBehaviour
{
	[Header("현재 챕터")] 
	[SerializeField] private EChapterType chapterType;
	
	private void Start()
	{
		ChapterMoveController.Instance.OnEnableController(chapterType);
	}
}
