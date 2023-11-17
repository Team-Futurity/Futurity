using FMODUnity;
using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using Animation = Spine.Animation;

public class BossPhaseEndCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private Animator bossAnimator;
	[SerializeField] private BossController bossController;
	[SerializeField] private UIDialogController dialogController;

	[Header("다이얼로그 데이터")] 
	[SerializeField] private DialogData dialogData;

	[Header("일러스트 컷 씬")] 
	[SerializeField] private SkeletonGraphic skeletonGraphic;
	
	[Header("Init Text")] 
	[SerializeField] private TextMeshProUGUI textField;
	[SerializeField] private string[] initText;
	[SerializeField] private float delayTime = 0.05f;
	[SerializeField] private float waitTime = 1.5f;

	[Header("Sound")] 
	[SerializeField] private ParamRef bgParamRef;

	private readonly int BOSS_DEATH_KEY = Animator.StringToHash("Death");
	private IEnumerator printText;
	private WaitForSeconds waitForSeconds;
	
	protected override void Init()
	{
		base.Init();
		
		waitForSeconds = new WaitForSeconds(delayTime);
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		
		AudioManager.Instance.SetParameterForBackgroundMusic(bgParamRef);
		bgParamRef.Value = 2;
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.SetActiveMainUI(true);
		bossController.isActive = true;
		
		dialogController.SetDialogData(dialogData);
		dialogController.Play();
	}
	
	public void StartScripting()
	{
		base.StartScripting();
	}

	public void StartSkeletonCutSceneWithText()
	{
		cutScene.Pause();
		StartCoroutine(PrintTextAndPlaySkeleton());
	}

	public void BossDeathAnimation(bool enable)
	{
		bossAnimator.SetBool(BOSS_DEATH_KEY, enable);
	}

	public void PlayBackGroundMusic()
	{
		AudioManager.Instance.SetParameterForBackgroundMusic(bgParamRef);
	}

	private IEnumerator PrintTextAndPlaySkeleton()
	{
		int curIndex = 0;

		while (curIndex < 2)
		{
			foreach (char text in initText[curIndex])
			{
				textField.text += text;
				yield return waitForSeconds;
			}

			yield return new WaitForSeconds(waitTime);
			textField.text = "";
			curIndex++;

			if (curIndex < 2)
			{
				Animation ani = skeletonGraphic.Skeleton.Data.Animations.Items[curIndex];
				skeletonGraphic.AnimationState.SetAnimation(0, ani, false);
			}
		}
		
		cutScene.Resume();
	}
}
