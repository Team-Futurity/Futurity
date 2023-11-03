using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private UIDialogController dialogController;
	[SerializeField] private GameObject chapterMoveTrigger;
	
	private TimelineManager timeline;
	
	private void Start()
	{
		timeline = TimelineManager.Instance;
	}

	public void InterimEvent(DialogData dialogData)
	{
		dialogController.gameObject.SetActive(true);
		dialogController.SetDialogData(dialogData);
		dialogController.Play();
	}

	public void SpawnerEndEvent(SpawnerManager manager, ESpawnerType spawnerType)
	{
		chapterMoveTrigger.SetActive(true);
		timeline.EnableCutScene(ECutSceneType.LASTKILL);
	}
}


