using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIButton : MonoBehaviour
{
	private Button button;

	public int layerOrder = 0;
	protected abstract void SelectAction();

	protected virtual void Awake()
	{
		TryGetComponent(out button);

		button.onClick.AddListener(() =>
		{
			Select();
		});
	}

	private void Start()
	{
		UIInputManager.Instance.AddButton(layerOrder, this);
	}

	public virtual void Select()
	{
		button.Select();

		SelectAction();
	}
}
