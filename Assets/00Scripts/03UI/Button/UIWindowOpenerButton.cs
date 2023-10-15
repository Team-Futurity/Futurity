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
		if (isClose)
		{
			UIManager.Instance.CloseWindow(OpenWindowType);
		}
		else
		{
			UIManager.Instance.OpenWindow(OpenWindowType);
		}
	}
}
