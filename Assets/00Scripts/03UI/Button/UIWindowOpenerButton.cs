using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectOpenerButton : UIButton
{
	[field: SerializeField]
	public WindowList windowType { get; private set; }

	protected override void ActiveFunc()
	{
		UIManager.Instance.OpenWindow(windowType);
	}
}
