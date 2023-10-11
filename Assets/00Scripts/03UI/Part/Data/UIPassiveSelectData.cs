using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassivePart Data", menuName = "PartData/Passive", order = 0)]
public class UIPassiveSelectData : ScriptableObject
{
	public Sprite partIconSpr;
	public Sprite partNameSpr;

	[TextArea]
	public string coreInfoText;

	[TextArea]
	public string subInfoText;
}
