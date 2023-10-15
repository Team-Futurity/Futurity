using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpenerButton : UIButton
{
	[field: SerializeField]
	public WindowList windowType { get; private set; }

	protected override void ActiveFunc()
	{
		UIManager.Instance.OpenWindow(windowType);
	}
}
