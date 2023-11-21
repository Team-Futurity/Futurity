using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIType
{
	NEXTSTAGE,
	OPENBOX,
	INTERACION,
}

public class ChapterMoveController : Singleton<ChapterMoveController>
{
	[Header("Component")]
	[ReadOnly(false), SerializeField] private ChapterCutSceneManager cutSceneManager;
	
	[Header("챕터 정보")] 
	[SerializeField] private List<ChapterData> chapterData;
	[SerializeField, ReadOnly(false)] private EChapterType curChapter = EChapterType.CHAPTER1_1;

	[Header("Fade Out 시간")] 
	[SerializeField] private float fadeOutTime = 0.5f;
	[SerializeField] private float fadeInTime = 1.0f;

	[Header("상호작용 UI")] 
	[SerializeField] private List<GameObject> interactionUI;

	[Header("에디터에서만 사용")] 
	[SerializeField] private EChapterType editorChapter;
	private ObjectPenetrate objectPenetrate;

	private void Start()
	{
#if UNITY_EDITOR
		curChapter = editorChapter;
#endif
		
		OnEnableController();
	}

	public void OnEnableController()
	{
		Init();
		CheckPenetrate();
		EnableEntryCutScene();
		StartUpdateHpGauge();

		Time.timeScale = 1.0f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F7))
		{
			MoveNextChapter();
		}
	}

	public void EnableInteractionUI(EUIType type) => interactionUI[(int)type].SetActive(true);
	public void DisableInteractionUI(EUIType type) => interactionUI[(int)type].SetActive(false);
	
	public void MoveNextChapter()
	{
		InputActionManager.Instance.DisableActionMap();

		FadeManager.Instance.FadeIn(fadeInTime, () =>
		{
			SceneLoader.Instance.LoadScene(chapterData[(int)curChapter].NextChapterName);
			
			curChapter++;
			objectPenetrate.enabled = false;
		});
	}

	public string GetCurrentChapter()
	{
		string curChapterName = "";

		switch (curChapter)
		{
			case EChapterType.CHAPTER1_1:
				curChapterName = ChapterSceneName.CHAPTER1_1;
				return curChapterName;
			
			case EChapterType.CHAPTER1_2:
				curChapterName = ChapterSceneName.CHAPTER1_2;
				return curChapterName;
			
			case EChapterType.CHAPTER1_3:
				curChapterName = ChapterSceneName.CHAPTER1_3;
				return curChapterName;
			
			case EChapterType.CHAPTER2_1:
				curChapterName = ChapterSceneName.CHAPTER2_1;
				return curChapterName;
			
			case EChapterType.CHAPTER2_2:
				curChapterName = ChapterSceneName.CHAPTER2_2;
				return curChapterName;
			
			case EChapterType.CHAPTER_BOSS:
				curChapterName = ChapterSceneName.BOSS_CHAPTER;
				return curChapterName;
			
			default:
				return curChapterName;
		}
	}

	#region UpdateHpGauge
	private void StartUpdateHpGauge()
	{
		if (chapterData[(int)curChapter].CutSceneType == ECutSceneType.NONE)
		{
			UpdateHpEvent();
			return;
		}

		StartCoroutine(WaitCutSceneEnd());
	}

	private IEnumerator WaitCutSceneEnd()
	{
		while (TimelineManager.Instance.isCutScenePlaying == true)
		{
			yield return null;
		}
		
		UpdateHpEvent();
	}

	private void UpdateHpEvent()
	{
		GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerData.status
			.updateHPEvent.Invoke(230f, 230f);
	}
	#endregion

	private void EnableEntryCutScene()
	{
		int index = (int)curChapter;
		if (chapterData[index].CutSceneType == ECutSceneType.NONE)
		{
			FadeManager.Instance.FadeOut(chapterData[index].FadeOutTime);
			return;
		}

		Action cutSceneEvent = null;

		TimelineManager.Instance.EnableCutScene(chapterData[index].CutSceneType);

		cutSceneEvent = () =>
		{
			TimelineManager.Instance.EnableNonPlayOnAwakeCutScene(chapterData[index].CutSceneType);
		};

		FadeManager.Instance.FadeOut(fadeOutTime, () => cutSceneEvent?.Invoke());
	}

	private void CheckPenetrate()
	{
		if (chapterData[(int)curChapter].IsPenetrate == true)
		{
			objectPenetrate.enabled = true;
			return;
		}

		objectPenetrate.enabled = false;
	}

	private void Init()
	{
		if (GameObject.FindWithTag("CutScene").TryGetComponent(out cutSceneManager) == true)
		{
			cutSceneManager.InitManager();
		}

		if (objectPenetrate != null)
		{
			return;
		}
		
		objectPenetrate = GameObject.FindWithTag("PlayerCamera").GetComponent<ObjectPenetrate>();
	}
}
