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
	// Source	: 어떤 Dialog에서 실행을 할 것인가?
	// Event	: Dialog의 어떤 이벤트에서 실행을 할 것인가?
	// Action	: 해당하는 Source의 Event에서 어떤 Action을 실행할 것인가? -> 지금은 Perfrom만

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
