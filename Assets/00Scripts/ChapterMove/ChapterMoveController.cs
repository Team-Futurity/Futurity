using System;
using System.Collections.Generic;
using UnityEngine;

public class ChapterMoveController : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private GameObject interactionUI;
	public void SetActiveInteractionUI(bool isActive) => interactionUI.SetActive(isActive);
	[ReadOnly(false), SerializeField] private ChapterCutSceneManager cutSceneManager;

	[Header("챕터 정보")] 
	[SerializeField] private List<ChapterData> chapterData;
	[SerializeField, ReadOnly(false)] private EChapterType curChapter = EChapterType.CHAPTER1_1;

	[Header("Fade Out 시간")] 
	[SerializeField] private float fadeOutTime = 0.5f;
	[SerializeField] private float fadeInTime = 1.0f;

	[Header("다음 씬으로 넘어갈 콜라이더")] 
	[SerializeField] private GameObject chapterMoveTrigger;
	
	[Header("디버그용 패널")] 
	[SerializeField] private bool isDebugMode;
	[SerializeField] private bool enableSpawner;
	[SerializeField] private List<SpawnerManager> spawnerManager;
	
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
		InputActionManager.Instance.DisableActionMap();
		
		FadeManager.Instance.FadeIn(fadeInTime, () =>
		{
			SceneLoader.Instance.LoadScene(chapterData[(int)curChapter].NextChapterName);
			curChapter++;
		});
	}

	private void EnableEntryCutScene()
	{
		int index = (int)curChapter;
		if (isDebugMode == true && chapterData[index].CutSceneType == ECutSceneType.NONE)
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

	private void Init()
	{
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
