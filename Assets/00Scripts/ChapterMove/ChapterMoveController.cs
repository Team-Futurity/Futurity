using System;
using System.Collections.Generic;
using UnityEngine;

public class ChapterMoveController : Singleton<ChapterMoveController>
{
	[Header("Component")] 
	[SerializeField] private GameObject interactionUI;
	[ReadOnly(false), SerializeField] private ChapterCutSceneManager cutSceneManager;
	
	[Header("챕터 정보")] 
	[SerializeField] private List<ChapterData> chapterData;
	[SerializeField, ReadOnly(false)] private EChapterType curChapter = EChapterType.CHAPTER1_1;

	[Header("Fade Out 시간")] 
	[SerializeField] private float fadeOutTime = 0.5f;
	[SerializeField] private float fadeInTime = 1.0f;

	[Header("에디터에서만 사용")] 
	[SerializeField] private EChapterType editorChapter;
	
	private ObjectPenetrate objectPenetrate;
	public void SetActiveInteractionUI(bool isActive) => interactionUI.SetActive(isActive);

	private void Start()
	{
		OnEnableController();
	}

	public void OnEnableController()
	{
		#if UNITY_EDITOR
		curChapter = editorChapter;
		#endif
		
		Init();
		CheckPenetrate();
		EnableEntryCutScene();
		
		GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerData.status
			.updateHPEvent.Invoke(230f, 230f);
		
		Time.timeScale = 1.0f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F7))
		{
			MoveNextChapter();
		}
	}
	
	public void MoveNextChapter()
	{
		InputActionManager.Instance.DisableActionMap();

		FadeManager.Instance.FadeIn(fadeInTime, () =>
		{
			SceneLoader.Instance.LoadScene(chapterData[(int)curChapter].NextChapterName);
			
			curChapter++;
			objectPenetrate.enabled = true;
		});
		
		Invoke(nameof(OnEnableController), fadeOutTime + 5.0f);
	}

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
		}
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
