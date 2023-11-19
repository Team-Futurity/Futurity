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
			if (OpenWindowType == WindowList.PAUSE)
			{
				Time.timeScale = 1f;
			}
			
			UIManager.Instance.CloseWindow(OpenWindowType);
			UIManager.Instance.RefreshWindow(UIManager.Instance.GetBefroeWindow());
		}
		else
		{
			UIManager.Instance.OpenWindow(OpenWindowType);
		}
	}
}
