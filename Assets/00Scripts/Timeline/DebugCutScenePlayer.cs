using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCutScenePlayer : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11))
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.BOSS_ENDPHASE);
		}

		if (Input.GetKeyDown(KeyCode.F12))
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.BOSS_DEATH);
		}
	}
}
