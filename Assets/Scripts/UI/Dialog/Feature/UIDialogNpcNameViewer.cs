using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogNpcNameViewer : UIDialogFeatureBase
{
	[field: SerializeField]
	public TMP_Text NpcNameTextField { get; private set; }

	protected override void UpdateFeature()
	{
		NpcNameTextField.text = dialogData.talker_Kor;
	}
}
