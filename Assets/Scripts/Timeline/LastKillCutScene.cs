using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKillCutScene : MonoBehaviour
{
	private void OnEnable()
	{
		TimelineManager.Instance.ChangeFollowTarget(true);
	}

	public void EndLastKillCutScene()
	{
		TimelineManager.Instance.ChangeFollowTarget(false);
		gameObject.SetActive(false);
	}
}
