using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDeathCutScene : CutSceneBase
{
	[Space(6)] 
	[Header("GayScale")]
	[SerializeField] private float grayScaleTime = 0.5f;
	private IEnumerator enableGrayScale;

	protected override void Init()
	{
		base.Init();
	}
	
	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		StartGrayScaleRoutine();
	}

	protected override void DisableCutScene()
	{
		chapterManager.playerCamera.RevertCameraValue();

		if (InputActionManager.Instance != null)
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions
				.UIBehaviour);
		}

		if (UIManager.Instance != null)
		{
			UIManager.Instance.OpenWindow(WindowList.GAME_OVER);	
		}

		chapterManager.GrayScale.amount.value = 0.0f;
		chapterManager.GrayScale.active = false;

		Time.timeScale = 1.0f;
	}
	
	private void StartGrayScaleRoutine()
	{
		chapterManager.GrayScale.active = true;
		
		enableGrayScale = EnableGrayScale();
		StartCoroutine(enableGrayScale);
	}
	
	private IEnumerator EnableGrayScale()
	{
		float time = 0.0f;

		while (time < grayScaleTime)
		{
			chapterManager.GrayScale.amount.value = Mathf.Lerp(chapterManager.GrayScale.amount.value, 1.0f, time / grayScaleTime);
			time += Time.unscaledDeltaTime;
			
			yield return null;
		}

		chapterManager.GrayScale.amount.value = 1.0f;
	}
}
