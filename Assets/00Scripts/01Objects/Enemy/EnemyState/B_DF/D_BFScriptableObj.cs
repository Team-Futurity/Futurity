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
	[Tooltip("���� ���� ��� Ÿ�̹�")] public float flooringTiming = 0f;
	[Tooltip("���� ���� ����Ʈ Ÿ�̹�")] public float atkEffectTiming = 0f;
	[Tooltip("���� ���� Ȱ��ȭ Ÿ�̹�")] public float atktTiming = 0f;
	[Tooltip("���� ���� ��Ȱ��ȭ Ÿ�̹�")] public float deActiveTiming = 0f;
	[Tooltip("���� ����")] public float attackSpeed = 0f;
	public float zFarDistance = 0f;
}