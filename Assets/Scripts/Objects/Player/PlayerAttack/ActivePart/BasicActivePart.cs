using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BasicActivePart : ActivePartProccessor
{
	public float minRange;  // cm 단위 (0.01unit)
	public float maxRange;	// cm 단위 (0.01unit)
	public float damage;
	public float duration;

	public override void GetPartData()
	{
		return;
	}
}
