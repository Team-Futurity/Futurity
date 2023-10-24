using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogDirectingSource
{
	public enum DialogEventType
	{
		NONE,

		BEFORE,
		START,
		STAY,
		END,

		MAX
	}
	// Source	: � Dialog���� ������ �� ���ΰ�?
	// Event	: Dialog�� � �̺�Ʈ���� ������ �� ���ΰ�?
	// Action	: �ش��ϴ� Source�� Event���� � Action�� ������ ���ΰ�? -> ������ Perfrom��

	public DialogData dialogSource;
	public DialogEventType eventType;
	public PerformDirectingSetting directingSetting;
}

[System.Serializable]
public class PerformDirectingSetting
{
	public enum PerformDirectingType
	{
		NONE,

		OPEN,
		CHANGE,

		MAX
	}

	public PerformDirectingType directType;
}
