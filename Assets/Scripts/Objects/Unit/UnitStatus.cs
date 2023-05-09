using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	[Header("1차 능력치")]
	[Tooltip("현 체력")]
	public float currentHp = 200f;
	[Tooltip("최대 체력")]
	public float maxHp = 200f;
	[Tooltip("이동속도")]
	public float speed = 3f;
	[Tooltip("최종 공격치")] 
	public float attack = 20f;
	[Tooltip("방어치")]
	public float defence = 5f;
	[Tooltip("크리티컬 확률")]
	[Range(0, 1)]
	public float criticalChance = 0f;
	[Tooltip("크리티컬 데미지 배율")]
	public float criticalDamageMultiplier = 0f;
}
