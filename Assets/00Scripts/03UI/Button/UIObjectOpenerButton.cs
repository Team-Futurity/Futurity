using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectOpenerButton : UIButton
{
	[field: SerializeField]
	public GameObject OpenObject { get; private set; }

	protected override void ActiveFunc()
	{
		OpenObject.SetActive(true);
	}
}
