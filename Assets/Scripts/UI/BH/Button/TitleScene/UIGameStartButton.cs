using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameStartButton : UIButton
{
	[field: SerializeField] public SceneKeyData sceneData { get; private set; }
	[field: SerializeField] public int loadingType { get; private set; }
	
	public override void Action()
	{
		SceneChangerController.SceneChange(sceneData, loadingType);
	}
}
