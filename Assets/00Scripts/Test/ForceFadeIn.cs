using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFadeIn : MonoBehaviour
{
	[SerializeField] private GameObject cutScene;
	private void Start()
	{
		FadeManager.Instance.FadeOut(1.0f);
		Invoke("PlayCutScene", 0.7f);
	}

	private void PlayCutScene()
	{
		//TimelineManager.Instance.EnableCutScene(ECutScene.BOSS_ENTRYCUTSCENE);
		cutScene.SetActive(true);
	}
}
