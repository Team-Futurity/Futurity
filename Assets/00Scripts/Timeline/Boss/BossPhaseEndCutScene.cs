using System.Collections;
using TMPro;
using UnityEngine;

public class BossPhaseEndCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private Animator bossAnimator;
	[SerializeField] private BossController bossController;
	
	[Header("Init Text")] 
	[SerializeField] private TextMeshProUGUI textField;
	[SerializeField] private string initText;
	[SerializeField] private float delayTime = 0.05f;

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
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.SetActiveMainUI(true);
		bossController.isActive = true;
	}
	
	public void StartScripting()
	{
		base.StartScripting();
	}

	public void StartStartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void BossDeathAnimation(bool enable)
	{
		bossAnimator.SetBool(BOSS_DEATH_KEY, enable);
	}
	
	public void StartPrintText()
	{
		textField.text = "";
		cutScene.Pause();

		printText = PrintText();
		StartCoroutine(printText);
	}

	private IEnumerator PrintText()
	{
		foreach (char text in initText)
		{
			textField.text += text;

			yield return waitForSeconds;
		}
		
		cutScene.Resume();
	}
}
