using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChapterMoveController : MonoBehaviour
{
	[Header("챕터 정보")] 
	[SerializeField] private EChapterType currentChapter;
	[SerializeField] private EChapterType nextChapter;

	[Header("Fade Out 시간")] 
	[SerializeField] private float fadeOutTime = 0.5f;
	[SerializeField] private float fadeInTime = 1.0f;

	[Header("디버그용 패널")] 
	[SerializeField] private bool isDebugMode;
	[SerializeField] private bool enableSpawner;
	[SerializeField] private List<SpawnerManager> spawnerManager;

	private GameObject player;
	private PlayerInput playerInput;

	private void Start()
	{
		Init();
		CheckDebugMode();
		EnableEntryCutScene();
	}

	public void MoveNextChapter()
	{
		switch (nextChapter)
		{
			case EChapterType.CHAPTER1_1:
				break;
			
			case EChapterType.CHAPTER1_2:
				ChangeChapter(ChapterSceneName.CHAPTER1_2);
				break;
			
			case EChapterType.CHAPTER_BOSS:
				ChangeChapter(ChapterSceneName.BOSS_CHAPTER);
				break;
			
			default:
				return;
		}
	}

	private void ChangeChapter(string sceneName)
	{
		playerInput.enabled = false;
		
		FadeManager.Instance.FadeIn(fadeInTime, () =>
		{
			SceneLoader.Instance.LoadScene(sceneName);
		});
	}

	private void EnableEntryCutScene()
	{
		if (isDebugMode == true)
		{
			return;
		}
		
		switch (currentChapter)
		{
			case EChapterType.CHAPTER1_1:
				// TODO : 새로운 인트로 컷신 결정 나면 거기서 제어
				break;
			
			case EChapterType.CHAPTER1_2:
				FadeManager.Instance.FadeOut(fadeOutTime, () =>
				{
					TimelineManager.Instance.Chapter1_Area2_EnableCutScene(EChapter1_2.AREA2_ENTRYSCENE);
				});
				break;
			
			case EChapterType.CHAPTER_BOSS:
				FadeManager.Instance.FadeOut(fadeOutTime, () =>
				{
					TimelineManager.Instance.BossStage_EnableCutScene(EBossCutScene.BOSS_ENTRYCUTSCENE);
				});
				break;
			
			default:
				return;
		}
	}

	private void Init()
	{
		player = GameObject.FindWithTag("Player");
		playerInput = player.GetComponent<PlayerInput>();
	}

	#region OnlyUseEditor

	private void CheckDebugMode()
	{
		if (isDebugMode == false)
		{
			return;
		}

		playerInput.enabled = true;

		if (enableSpawner == false)
		{
			return;
		}

		foreach (SpawnerManager manager in spawnerManager)
		{
			manager.SpawnEnemy();
		}
	}
	
	#endregion
}
