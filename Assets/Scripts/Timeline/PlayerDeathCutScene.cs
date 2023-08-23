using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDeathCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject[] disableUI;
	[SerializeField] [Tooltip("플레이어 사망시 호출할 Window")] private GameObject deathOpenWindow;
	[SerializeField] [Tooltip("panel을 생성할 canvas")] private Canvas panelCanvas;

	[Header("흑백 전환 시간")] 
	[SerializeField] private float grayScaleTime = 0.5f;
	
	private GrayScale grayScale = null;
	private IEnumerator enableGrayScale;

	protected override void Init()
	{
		if (grayScale == null)
		{
			Camera.main.GetComponent<Volume>().profile.TryGet<GrayScale>(out grayScale);
		}
	}
	
	protected override void EnableCutScene()
	{
		foreach (var ui in disableUI)
		{
			ui.gameObject.SetActive(false);		
		}
		
		StartGrayScaleRoutine();
	}

	public override void DisableCutScene()
	{
		WindowManager.Instance.WindowOpen(deathOpenWindow, panelCanvas.transform, false, 
			Vector2.zero, Vector2.one);
		
		TimelineManager.Instance.ResetCameraValue();
		
		grayScale.amount.value = 0.0f;
		grayScale.active = false;
	}
	
	private void StartGrayScaleRoutine()
	{
		grayScale.active = true;
		
		enableGrayScale = EnableGrayScale();
		StartCoroutine(enableGrayScale);
	}
	
	private IEnumerator EnableGrayScale()
	{
		float time = 0.0f;

		while (time < grayScaleTime)
		{
			grayScale.amount.value = Mathf.Lerp(grayScale.amount.value, 1.0f, time / grayScaleTime);
			time += Time.unscaledDeltaTime;
			
			yield return null;
		}

		grayScale.amount.value = 1.0f;
	}
}
