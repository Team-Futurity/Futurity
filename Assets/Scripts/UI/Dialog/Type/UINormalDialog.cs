using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINormalDialog : MonoBehaviour
{
	[field: SerializeField] public TMP_Text NpcNameText { get; private set; }

	public void SetName(string npcName)
	{
		NpcNameText.text = npcName;
	}
}
