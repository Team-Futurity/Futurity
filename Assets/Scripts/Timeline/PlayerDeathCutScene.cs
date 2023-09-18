using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDeathCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject[] disableUI;
	[SerializeField] private GameObject gameOverUI;

	[Header("흑백 전환 시간")] 
	[SerializeField] private float grayScaleTime = 0.5f;

	[Header("사망 거리 저장")]
	[SerializeField] private Transform endPos;
	[SerializeField] private Transform newFollowTarget;
	
	private GrayScale grayScale = null;
	private IEnumerator enableGrayScale;

	protected override void Init()
	{
		if (grayScale == null && Camera.main != null)
		{
			Camera.main.GetComponent<Volume>().profile.TryGet<GrayScale>(out grayScale);
		}
	}
	
	protected override void EnableCutScene()
	{
		endPos.position = TimelineManager.Instance.GetOffsetVector(-3.0f);
		
		TimelineManager.Instance.ChangeFollowTarget(true, newFollowTarget);
		GameObject.FindWithTag("Player").GetComponent<Animator>().Play("New State", -1, 0f);
		
		foreach (var ui in disableUI)
		{
			ui.gameObject.SetActive(false);		
		}
		
		StartGrayScaleRoutine();
	}

	public override void DisableCutScene()
	{
		TimelineManager.Instance.ResetCameraValue();
		TimelineManager.Instance.ChangeFollowTarget();
		
		//gameOverUI.SetActive(true);
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
