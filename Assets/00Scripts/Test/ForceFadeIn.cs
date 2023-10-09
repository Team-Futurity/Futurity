using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFadeIn : MonoBehaviour
{
	[SerializeField] private BossController boss;
	private void Start()
	{
		FadeManager.Instance.FadeOut(1.0f, () => boss.isActive = true);
		//Invoke("PlayCutScene", 0.7f);
	}

	private void PlayCutScene()
	{
		TimelineManager.Instance.EnableCutScene(ECutScene.BOSS_ENTRYCUTSCENE);
	}
}
