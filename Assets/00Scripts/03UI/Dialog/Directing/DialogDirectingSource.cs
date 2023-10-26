using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogDirectingSource
{
	public enum DialogEventType
	{
		NONE,

		START,
		END,

		MAX
	}
	// Source	: 어떤 Dialog에서 실행을 할 것인가?
	// Event	: Dialog의 어떤 이벤트에서 실행을 할 것인가?
	// Action	: 해당하는 Source의 Event에서 어떤 Action을 실행할 것인가? -> 지금은 Perfrom만
	//			  무조건 Open 아니면 안됨..ㅎ

	public DialogData dialogSource;
	public DialogEventType eventType;
	public UIPerformBoard board;
}

