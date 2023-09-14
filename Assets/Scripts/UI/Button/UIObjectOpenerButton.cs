using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectOpenerButton : UIButton
{
	protected override void SelectAction()
	{
		Debug.Log("SELECT" + GetType());

	}
}
