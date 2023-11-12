using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1_A3_CheckInteraction : MonoBehaviour
{
	private readonly ECutSceneType[] curCutScene = new ECutSceneType[]
	{
		ECutSceneType.CHAPTER1_AREA3_SPAWN_1, ECutSceneType.CHAPTER1_AREA3_SPAWN_2
	};
	private int curIndex = 0;

	public void CheckInteraction()
	{
		TimelineManager.Instance.EnableCutScene(curCutScene[curIndex++]);
	}
}
