using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StateData", menuName = "ScriptableObject/Unit/D_BFStateData")]
public class D_BFScriptableObj : ScriptableObject
{
	public D_BFData data;
}

[Serializable]
public class D_BFData
{
	[Tooltip("개별 장판 출력 타이밍")] public float flooringTiming = 0f;
	[Tooltip("개별 공격 이펙트 타이밍")] public float atkEffectTiming = 0f;
	[Tooltip("개별 공격 활성화 타이밍")] public float atktTiming = 0f;
	[Tooltip("개별 공격 비활성화 타이밍")] public float deActiveTiming = 0f;
	[Tooltip("공격 간격")] public float attackSpeed = 0f;
	public float zFarDistance = 0f;
}