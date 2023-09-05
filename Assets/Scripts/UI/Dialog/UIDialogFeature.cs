using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class UIDialogController
{
	private Dictionary<UIDialogType, Action> dataSetUpDic; 

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

	private void DataSetUp()
	{
		dataSetUpDic = new Dictionary<UIDialogType, Action>();
		
		dataSetUpDic.Add(UIDialogType.NORMAL, SetNormalData);
		dataSetUpDic.Add(UIDialogType.RANGE, SetRangeData);
		dataSetUpDic.Add(UIDialogType.CUTSCENE, SetCutSceneData);
		dataSetUpDic.Add(UIDialogType.STANDING, SetStandingData);

		dataSetUpDic[DialogType]();
	}

	#region Normal

	private UINormalDialog normalDialog;

	private void SetNormalData()
	{
		TryGetComponent(out normalDialog);
		normalDialog.NpcNameText.SetText("");
		
		onShow?.AddListener(SetNormalName);
	}

	private void SetNormalName(DialogData data)
	{
		normalDialog.NpcNameText.SetText(data.talker_Kor);
	}

	#endregion

	#region Range

	private void SetRangeData()
	{
		
	}

	#endregion

	#region CutScene

	private void SetCutSceneData()
	{
		
	}
	
	#endregion

	#region Standing

	private void SetStandingData()
	{
		
	}

	#endregion
}