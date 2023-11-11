using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AutoSkipButton : MonoBehaviour
{
	[Header("Component : 수동할당")] 
	[SerializeField] private bool isTutorial = false;
	[SerializeField] private Button skipButton;
	[SerializeField] private Button autoButton;
	
	[Header("Component : 자동할당")]
	[SerializeField, ReadOnly(false)] private TimelineScripting timelineScripting;
	
	[Header("현재 재생중인 컷 씬")] 
	[SerializeField, ReadOnly(false)] private PlayableDirector playCutScene;

	[Header("설정값")] 
	[SerializeField] private float skipSpeed = 100.0f;
	[SerializeField] private float fadeInTime = 0.5f;

	public void InitAutoSkipButton(TimelineScripting scripting) => timelineScripting = scripting;
	public void InitPlayCutScene(PlayableDirector cutScene) => playCutScene = cutScene;
	
	public void SkipCutScene()
	{
		if (isTutorial == true)
		{
			SceneLoader.Instance.LoadScene(ChapterSceneName.CHAPTER1_1);	
		}
		
		if (playCutScene == null)
		{
			return;
		}

		skipButton.interactable = false;
		InputActionManager.Instance.DisableActionMap();

		FadeManager.Instance.FadeIn(fadeInTime, () =>
		{
			timelineScripting.isSkip = true;
			skipButton.interactable = true;
			
			playCutScene.playableGraph.GetRootPlayable(0).SetSpeed(skipSpeed);
		});
	}
}
