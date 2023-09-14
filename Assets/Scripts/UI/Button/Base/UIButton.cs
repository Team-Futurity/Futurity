using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIButton : MonoBehaviour
{
	private Button button;

	public int layerOrder = 0;
	protected abstract void ActiveAction();

	protected virtual void Awake()
	{
		TryGetComponent(out button);

		button.onClick.AddListener(() =>
		{
			Select();

			ActiveAction();
		});
	}

	private void Start()
	{
		UIInputManager.Instance.AddButton(layerOrder, this);
	}

	public void Active()
	{
		ActiveAction();
	}

	public void Select()
	{
		button.Select();
	}

	public Button GetButton()
	{
		return button;
	}
}
