using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExitButton : UIButton
{
	private WindowOpenController windowRemote;

	private void Awake()
	{
		TryGetComponent(out windowRemote);
	}

	public override void Action()
	{
		windowRemote.WindowDeactiveOpen();
	}
}
