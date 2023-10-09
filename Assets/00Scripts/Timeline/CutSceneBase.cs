using System;
using UnityEngine;

public abstract class CutSceneBase : MonoBehaviour
{
	[SerializeField] protected ChapterCutSceneManager chapterManager;
	protected virtual void Init() { }
	protected virtual void EnableCutScene() { }
	public abstract void DisableCutScene();

	private void OnEnable()
	{
		chapterManager = gameObject.GetComponentInParent<ChapterCutSceneManager>();
		
		Init();
		EnableCutScene();
	}

	private void OnDisable()
	{
		DisableCutScene();
	}
}
