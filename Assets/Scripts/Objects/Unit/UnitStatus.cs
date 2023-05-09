using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	[Header("1�� �ɷ�ġ")]
	[Tooltip("�� ü��")]
	public float currentHp = 200f;
	[Tooltip("�ִ� ü��")]
	public float maxHp = 200f;
	[Tooltip("�̵��ӵ�")]
	public float speed = 3f;
	[Tooltip("���� ����ġ")] 
	public float attack = 20f;
	[Tooltip("���ġ")]
	public float defence = 5f;
	[Tooltip("ũ��Ƽ�� Ȯ��")]
	[Range(0, 1)]
	public float criticalChance = 0f;
	[Tooltip("ũ��Ƽ�� ������ ����")]
	public float criticalDamageMultiplier = 0f;
}
