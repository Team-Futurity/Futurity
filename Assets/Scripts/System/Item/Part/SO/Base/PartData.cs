using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PartData", menuName ="Part/ItemData", order = 0)]
public class PartData : ScriptableObject
{
	[field: SerializeField] public int PartCode { get; private set; }

	[field: SerializeField] public PartTriggerType PartType { get; private set; }

	// Passive
	[field: SerializeField] public PassivePartGrade PartGrade { get; private set; }

	[field: SerializeField] public float PartActivaton { get; private set; }
}
