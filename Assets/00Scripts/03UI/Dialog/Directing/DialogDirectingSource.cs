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
	// Source	: � Dialog���� ������ �� ���ΰ�?
	// Event	: Dialog�� � �̺�Ʈ���� ������ �� ���ΰ�?
	// Action	: �ش��ϴ� Source�� Event���� � Action�� ������ ���ΰ�? -> ������ Perfrom��
	//			  ������ Open �ƴϸ� �ȵ�..��

	public DialogData dialogSource;
	public DialogEventType eventType;
	public UIPerformBoard board;
}

