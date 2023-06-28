using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreditButton : UIButton
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
