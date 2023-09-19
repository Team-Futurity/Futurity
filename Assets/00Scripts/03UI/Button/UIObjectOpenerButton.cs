using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectOpenerButton : UIButton
{
	[field: SerializeField]
	public GameObject OpenPrefab { get; private set; }

	protected override void ActiveAction()
	{
		Debug.Log("SELECT" + GetType());

	}
}
