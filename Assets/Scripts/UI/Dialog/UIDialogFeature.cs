using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class UIDialogController
{
	private Dictionary<UIDialogType, Action> dataSetUpDic;
	private UIDialogType beforeType = UIDialogType.NONE;

	private readonly Dictionary<UIDialogType, string[]> extensionNameByType = new Dictionary<UIDialogType, string[]>
	{
		{ UIDialogType.NORMAL , new string[] {"UIDialogNpcNameViewer", "UIDialogPass" } },
		{ UIDialogType.STANDING, new string[] { "UIDialogImageChanger", "UIDialogNpcNameViewer", "UIDialogPass", "UIDialogSkip"} },
		{ UIDialogType.RANGE, new string[] { "UIDialogDistance"} },
		{ UIDialogType.CUTSCENE, new string[] { "UIDialogImageChanger", "UIDialogNpcNameViewer", "UIDialogPass", "UIDialogSkip" } }
	};

	#region Extension Settings
	private void OnValidate()
	{
		if (DialogType == UIDialogType.NONE || DialogType == UIDialogType.MAX || Application.isPlaying)
			return;

		RemoveScript(beforeType);

		beforeType = DialogType;

		AddScript(DialogType);
	}

	private void AddScript(UIDialogType type)
	{
		// extension에 맞는 Type List를 String으로 가져온다.
		var nameByType = extensionNameByType[type];

		// 순회하면서 기능 붙이기
		for (int i = 0; i < nameByType.Length; ++i)
		{
			Type extensionType = Type.GetType(nameByType[i]);

			gameObject.AddComponent(extensionType);
		}
	}

	private void RemoveScript(UIDialogType type)
	{
		var nameByType = extensionNameByType[type];

		for (int i = 0; i < nameByType.Length; ++i)
		{
			Type extensionType = Type.GetType(nameByType[i]);

			if (CheckScript(extensionType))
			{
				UnityEditor.EditorApplication.delayCall += () =>
				{
					DestroyImmediate(GetComponent(extensionType));
				};
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

}