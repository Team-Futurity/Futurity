using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_EXITCUTSCENE);
		}
	}
}
