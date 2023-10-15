using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector introCutScene;
	
	[Header("Fade Time")] 
	[SerializeField] private float fadeInTime = 0.8f;
	[SerializeField] private float fadeOutTime = 0.8f;

	[Header("다음으로 이동할 씬 이름")] 
	[SerializeField] private string nextSceneName;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
	}

	public override void DisableCutScene()
	{
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName);
		});
	}
	
	public void Intro_PrintScripts()
	{
		
	}
}
