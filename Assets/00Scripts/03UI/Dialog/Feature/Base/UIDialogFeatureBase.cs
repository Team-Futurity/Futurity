using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIDialogFeatureBase : MonoBehaviour
{
	protected UIDialogController controller;
	protected DialogDataGroup dialogData;

	protected virtual void Awake()
	{
		TryGetComponent(out controller);

		controller.OnShow?.AddListener(UpdateDialogData);
	}

	private void UpdateDialogData(DialogDataGroup data)
	{
		dialogData = data;

		UpdateFeature();
	}

	protected virtual void UpdateFeature()
	{

	}
}
