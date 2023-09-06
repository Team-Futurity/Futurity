using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHitGauge : MonoBehaviour
{
	[field: SerializeField]
	public UIGauge Gauge { get; private set; }
	
	[field: SerializeField]
	public HitCountSystem CountSystem { get; private set; }

	private void Awake()
	{
	}
}
