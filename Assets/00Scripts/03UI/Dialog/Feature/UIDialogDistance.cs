using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogDistance : UIDialogFeatureBase
{
	[field: SerializeField]
	public float OpenDist { get; private set; }
	
	[field: SerializeField]
	public Transform TargetPos { get; private set; }
	
	[field: SerializeField]
	public LayerMask TargetLayer { get; private set; }

	public bool isGizmosView;
	private bool isInCollider;

	private bool isOpen;
	
	
	protected override void Awake()
	{
		base.Awake();
		
		isInCollider = false;
		isOpen = false;
	}

	private void Update()
	{
		if (isInCollider && !isOpen)
		{
			isOpen = true;
			
			// controller.ShowDialog();
			controller.PlayDialog();
		}

		if (!isInCollider)
		{
			isOpen = false;
			
			// controller.OnPass();
		}
	}

	private void FixedUpdate()
	{
		var targetArray = Physics.OverlapSphere(TargetPos.position, OpenDist, TargetLayer);

		isInCollider = (targetArray == null);
	}

	private void OnDrawGizmos()
	{
		if (!isGizmosView)
		{
			return;
		}

		Gizmos.color = (isInCollider) ? Color.red : Color.green;
		Gizmos.DrawWireSphere(TargetPos.position, OpenDist);
	}
}
