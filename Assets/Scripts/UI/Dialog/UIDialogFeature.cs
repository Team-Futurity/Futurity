using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class UIDialogController
{
	private dynamic baseScript;

	private readonly string[] extensionName =
	{
		"UINormalDialog", "UIStandingDialog", "UIRangeDialog", "UICutSceneDialog"
	};

	#region Extension Settings
	private void OnValidate()
	{
		if (DialogType == UIDialogType.NONE || DialogType == UIDialogType.MAX || Application.isPlaying)
			return;

		switch (DialogType)
		{
			case UIDialogType.NORMAL:
				AddScript<UINormalDialog>();
				break;

			case UIDialogType.CUTSCENE:
				AddScript<UICutSceneDialog>();
				break;

			case UIDialogType.RANGE:
				AddScript<UIRangeDialog>();
				break;

			case UIDialogType.STANDING:
				AddScript<UIStandingDialog>();
				break;
		}
	}

	private void AddScript<T>() where T : Component
	{
		for (int i = 0; i < extensionName.Length; ++i)
		{
			Type extensionType = Type.GetType(extensionName[i]);

			if (typeof(T) == extensionType)
			{
				if (CheckScript<T>())
				{
					continue;
				}
				else
				{
					gameObject.AddComponent<T>();
				}
			}
			else
			{
				if (CheckScript(extensionType))
				{
					UnityEditor.EditorApplication.delayCall += () =>
					{
						DestroyImmediate(GetComponent(extensionType));
					};
				}
			}
		}
	}

	private bool CheckScript<T>() where T : Component
	{
		return (GetComponentsInChildren<T>().FirstOrDefault() != null);
	}

	private bool CheckScript(Type checkType)
	{
		return (GetComponentsInChildren(checkType).FirstOrDefault() != null);
	}
	
	#endregion

	private void DataSetUp(UIDialogType type)
	{
		switch (type)
		{
			case UIDialogType.NORMAL:
				baseScript = GetComponent<UINormalDialog>();
				break;

			case UIDialogType.CUTSCENE:
				baseScript = GetComponent<UICutSceneDialog>();
				break;

			case UIDialogType.RANGE:
				baseScript = GetComponent<UIRangeDialog>();
				break;

			case UIDialogType.STANDING:
				baseScript = GetComponent<UIStandingDialog>();
				break;
		}
	}

	#region Normal

	private void SetName()
	{
	}

	#endregion

	#region Range

	#endregion

	#region CutScene

	#endregion

	#region Standing

	#endregion
}