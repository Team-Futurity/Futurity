using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPartEquipSelect : MonoBehaviour
{
	[SerializeField]
	private UIYesNoButton[] buttons = new UIYesNoButton[2];

	[HideInInspector]
	public UnityEvent<bool> onClose;

	public bool isExitMode = false;

	private void Awake()
	{
	}

	public void SetExitMode()
	{
		UIInputManager.Instance.SetUnableMoveButton(false);

		buttons[0].onSelected?.AddListener((x) =>
		{
			UIManager.Instance.CloseWindow(WindowList.PART_EXIT);
			UIManager.Instance.CloseWindow(WindowList.PART_EQUIP);
		});

		buttons[1].onSelected?.AddListener((x) =>
		{
			UIManager.Instance.CloseWindow(WindowList.PART_EXIT);

			UIManager.Instance.RefreshWindow(UIManager.Instance.GetBefroeWindow());
			UIInputManager.Instance.SetSaveIndexToCurrentIndex();
		});
	}

	public void SetNormalMode()
	{
		for (int i = 0; i < buttons.Length; ++i)
		{
			buttons[i].onSelected?.AddListener(OnActiveButton);
		}
	}

	private void OnActiveButton(bool isEquip)
	{
		UIManager.Instance.CloseWindow(WindowList.PART_EQUIP_SELECT);
		onClose?.Invoke(isEquip);
	}
}