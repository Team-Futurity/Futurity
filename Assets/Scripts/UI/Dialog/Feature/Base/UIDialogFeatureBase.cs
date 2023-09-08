using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIDialogFeatureBase : MonoBehaviour
{
	protected UIDialogController controller;
	protected DialogData dialogData;

	protected virtual void Awake()
	{
		TryGetComponent(out controller);

		controller.OnShow?.AddListener(UpdateDialogData);
		controller.OnPlay?.AddListener(UpdateDialogData);
	}

	private void UpdateDialogData(DialogData data)
	{
		dialogData = data;

		UpdateFeature();
	}

	protected virtual void UpdateFeature()
	{

	}
}
