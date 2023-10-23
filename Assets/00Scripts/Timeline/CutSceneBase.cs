using System;
using UnityEngine;

public abstract class CutSceneBase : MonoBehaviour
{
	[SerializeField] protected ChapterCutSceneManager chapterManager;
	protected virtual void Init() { }
	protected virtual void EnableCutScene() { }
	protected abstract void DisableCutScene();

	private void OnEnable()
	{
		chapterManager = gameObject.GetComponentInParent<ChapterCutSceneManager>();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		
		Init();
		EnableCutScene();
	}

	private void OnDisable()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		DisableCutScene();
	}
}
