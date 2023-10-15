using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameExitButton : UIButton
{
	protected override void ActiveFunc()
	{
		Application.Quit();
	}
}
