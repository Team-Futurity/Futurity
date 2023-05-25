using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PartData", menuName ="Part/ItemData", order = 0)]
public class PartData : ScriptableObject
{
	[field: Header("��ǰ �ڵ�")]
	[field: SerializeField] public int PartCode { get; private set; }

	[field: Header("��Ƽ�� or �нú� ����")]
	[field: SerializeField] public PartTriggerType PartType { get; private set; }

	[field: Header("��ǰ ���")]
	[field: SerializeField] public PassivePartGrade PartGrade { get; private set; }

	[field: Header("�ɷ� Ȱ��ȭ �ۼ�Ʈ")]
	[field: SerializeField] public float PartActivation { get; private set; }
}
