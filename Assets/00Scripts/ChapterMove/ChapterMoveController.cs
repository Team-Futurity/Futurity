using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ChapterMoveController : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private GameObject interactionUI;
	public void SetActiveInteractionUI(bool isActive) => interactionUI.SetActive(isActive);
	[SerializeField] private ChapterCutSceneManager cutSceneManager;
	
	[Header("챕터 정보")] 
	[SerializeField] private EChapterType currentChapter;
	[SerializeField] private EChapterType nextChapter;
	public EChapterType CurrentChapter => currentChapter;

	[Header("Fade Out 시간")] 
	[SerializeField] private float fadeOutTime = 0.5f;
	[SerializeField] private float fadeInTime = 1.0f;

	[Header("다음 씬으로 넘어갈 콜라이더")] 
	[SerializeField] private GameObject chapterMoveTrigger;
	
	[Header("디버그용 패널")] 
	[SerializeField] private bool isDebugMode;
	[SerializeField] private bool enableSpawner;
	[SerializeField] private List<SpawnerManager> spawnerManager;

	private GameObject player;

	private void Start()
	{
		Init();
		CheckDebugMode();
		EnableEntryCutScene();

		GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerData.status.updateHPEvent.Invoke(230f, 230f);
		Time.timeScale = 1.0f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F7))
		{
			MoveNextChapter();
		}
	}
	
	public void EnableExitCollider()
	{
		if (chapterMoveTrigger == null)
		{
			return;
		}
		chapterMoveTrigger.SetActive(true);
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
		InputActionManager.Instance.DisableActionMap();
		
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

		Action cutSceneEvent = null;
		
		switch (currentChapter)
		{
			case EChapterType.CHAPTER1_1:
				TimelineManager.Instance.Chapter1_Area1_EnableCutScene(EChapter1CutScene.AREA1_ENTRYCUTSCENE);
				cutSceneEvent = () =>
				{
					TimelineManager.Instance.ChapterScene[(int)EChapter1CutScene.AREA1_ENTRYCUTSCENE].
						GetComponent<PlayableDirector>().Play();
				};
				break;
			
			case EChapterType.CHAPTER1_2:
				TimelineManager.Instance.Chapter1_Area2_EnableCutScene(EChapter1_2.AREA2_ENTRYSCENE);
				cutSceneEvent = () =>
				{
					TimelineManager.Instance.ChapterScene[(int)EChapter1_2.AREA2_ENTRYSCENE].
						GetComponent<PlayableDirector>().Play();
				};
				break;
			
			case EChapterType.CHAPTER_BOSS:
				TimelineManager.Instance.BossStage_EnableCutScene(EBossCutScene.BOSS_ENTRYCUTSCENE);
				cutSceneEvent = () =>
				{
					TimelineManager.Instance.ChapterScene[(int)EBossCutScene.BOSS_ENTRYCUTSCENE]
						.GetComponent<PlayableDirector>().Play();
				};
				break;
			
			case EChapterType.NONEVENTCHAPTER:
				break;
			
			default:
				return;
		}

		FadeManager.Instance.FadeOut(fadeOutTime, () => cutSceneEvent?.Invoke());
	}

	private void Init()
	{
		player = GameObject.FindWithTag("Player");

		if (GameObject.FindWithTag("CutScene").TryGetComponent(out cutSceneManager) == true)
		{
			cutSceneManager.InitManager();
		}
	}

	#region OnlyUseEditor

	private void CheckDebugMode()
	{
		if (isDebugMode == false)
		{
			return;
		}
		
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
