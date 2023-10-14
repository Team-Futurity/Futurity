using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public List<DialogData> dialogs = new List<DialogData>();

	public Image textImage;
	
	
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

			controller.SetDialogData(GetDialogData());
			controller.PlayDialog();
			textImage.gameObject.SetActive(isOpen);
		}

		if (!isInCollider)
		{
			isOpen = false;

			textImage.gameObject.SetActive(isOpen);
		}

		
	}

	private void LateUpdate()
	{
		var targetArray = Physics.OverlapSphere(TargetPos.position, OpenDist, TargetLayer);

		if (targetArray.Length <= 0)
		{
			isInCollider = false;
			return;
		}

		isInCollider = (targetArray[0].transform.name.Contains("PlayerPrefab"));
	}

	private DialogData GetDialogData()
	{
		int selectIndex = UnityEngine.Random.Range(0, dialogs.Count);
		return dialogs[selectIndex];
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
