using System;
using System.Collections.Generic;
using UnityEngine;

public enum EUIType
{
	NEXTSTAGE,
	OPENBOX,
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
		
		GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerData.status
			.updateHPEvent.Invoke(230f, 230f);
		
		Time.timeScale = 1.0f;
	}

	public void EnableInteractionUI(EUIType type) => interactionUI[(int)type].SetActive(true);
	public void DisableInteractionUI(EUIType type) => interactionUI[(int)type].SetActive(false);
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F7))
		{
			MoveNextChapter();
		}
		
		if (Input.GetKeyDown(KeyCode.M))
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.ACTIVE_BETA);
		}
		
		if (Input.GetKeyDown(KeyCode.N))
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.ACTIVE_ALPHA);
		}
	}
	
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
