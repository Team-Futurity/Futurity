using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpenerButton : UIButton
{
	[field: SerializeField]
	public WindowList OpenWindowType { get; private set; }

	public bool isClose = false;

	protected override void ActiveFunc()
	{
		UIInputManager.Instance.SaveIndex();
		
		if (isClose)
		{
			UIManager.Instance.CloseWindow(OpenWindowType);
			UIManager.Instance.RefreshWindow(WindowList.TITLE);
			UIInputManager.Instance.SetSaveIndexToCurrentIndex();
		}
		else
		{
			UIManager.Instance.OpenWindow(OpenWindowType);
		}
	}
}
