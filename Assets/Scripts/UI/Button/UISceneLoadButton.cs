using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	protected override void SelectAction()
	{
		Debug.Log("SELECT" + GetType());

	}
}
