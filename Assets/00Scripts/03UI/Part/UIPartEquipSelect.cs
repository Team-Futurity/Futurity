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

	private void Awake()
	{
		for (int i = 0; i < buttons.Length; ++i)
		{
			buttons[i].onActive?.AddListener(OnActiveButton);
		}
	}

	private void OnActiveButton(bool isNo)
	{
		UIManager.Instance.CloseWindow(WindowList.PART_EQUIP_SELECT);
		onClose?.Invoke(isNo);
	}
}