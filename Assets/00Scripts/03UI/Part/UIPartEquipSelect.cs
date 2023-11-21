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

	private bool initExitMode = false;

	private void Update()
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PART_EXIT) && isExitMode && !initExitMode)
		{
			initExitMode = true;
			
			SetExitMode();
		}
	}

	public void SetExitMode()
	{
		Init();
		

		buttons[0].onSelected?.AddListener((x) =>
		{
			UIManager.Instance.CloseWindow(WindowList.PART_EXIT);
			UIManager.Instance.CloseWindow(WindowList.PART_EQUIP);
			
			UIInputManager.Instance.InitSaveIndex();

			initExitMode = false;
		});

		buttons[1].onSelected?.AddListener((x) =>
		{
			UIManager.Instance.CloseWindow(WindowList.PART_EXIT);

			UIManager.Instance.RefreshWindow(UIManager.Instance.GetBefroeWindow());
			UIInputManager.Instance.SetSaveIndexToCurrentIndex();
			initExitMode = false;
		});
	}

	public void SetNormalMode()
	{
		Init();

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

	private void Init()
	{
		UIInputManager.Instance.SetUnableMoveButton(false);

		for (int i = 0; i < buttons.Length; ++i)
		{
			buttons[i].onSelected?.RemoveAllListeners();
		}
	}
}