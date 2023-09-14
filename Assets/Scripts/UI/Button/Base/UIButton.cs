using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIButton : MonoBehaviour
{
	private Button button;

	public int layerOrder = 0;

	protected virtual void Awake()
	{
		TryGetComponent(out button);

		button.onClick.AddListener(() =>
		{
			Select();
		});
	}

	public virtual void Select()
	{
		button.Select();

		SelectAction();
	}
	protected abstract void SelectAction();
}
