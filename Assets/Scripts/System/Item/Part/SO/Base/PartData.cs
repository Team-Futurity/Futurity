using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PartData", menuName ="Part/ItemData", order = 0)]
public class PartData : ScriptableObject
{
	[field: Header("부품 코드")]
	[field: SerializeField] public int PartCode { get; private set; }

	[field: Header("액티브 or 패시브 여부")]
	[field: SerializeField] public PartTriggerType PartType { get; private set; }

	[field: Header("부품 등급")]
	[field: SerializeField] public PassivePartGrade PartGrade { get; private set; }

	[field: Header("능력 활성화 퍼센트")]
	[field: SerializeField] public float PartActivation { get; private set; }
}
