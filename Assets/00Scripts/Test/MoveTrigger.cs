using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			FadeManager.Instance.FadeIn();
			
			Invoke("MovePlayer", 1.3f);
		}
	}

	private void MovePlayer()
	{
		SteageMove.Instance.MoveStage(SteageMove.EStageType.STAGE_3);
		FadeManager.Instance.FadeOut();
		TimelineManager.Instance.EnableCutScene(ECutScene.AREA3_ENTRYCUTSCENE);
	}
}
