using System;
using UnityEngine;

public abstract class CutSceneBase : MonoBehaviour
{
	protected virtual void Init() { }
	protected virtual void EnableCutScene() { }
	public abstract void DisableCutScene();

	private void OnEnable()
	{
		Init();
		EnableCutScene();
	}
}
