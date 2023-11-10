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
		NEXT_CHANGE_START,
		END,
		NEXT_CHANGE_END,

		MAX
	}
	
	public DialogData dialogSource;
	public DialogEventType eventType;
	public UIPerformBoard board;
}

