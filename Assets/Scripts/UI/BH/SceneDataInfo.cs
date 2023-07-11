using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataInfo : MonoBehaviour
{
	[field: SerializeField] public SceneKeyData sceneData { get; private set; }
	[field: SerializeField] public int loadingType { get; private set; }


	public void SceneChanger()
	{
		SceneChangerController.SceneChange(sceneData, loadingType);
	}
	
}
