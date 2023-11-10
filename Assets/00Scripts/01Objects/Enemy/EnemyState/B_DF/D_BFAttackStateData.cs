using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateData", menuName = "ScriptableObject/Unit/D_BFStateData")]
public class D_BFAttackStateData : StateData
{
	public float zFarDistance;

	public float flooringTiming = 0f;
	public float atkTiming = 0f;
	public float deActiveTiming = 0f;
	public float attackSpeed = 0f;

	public override void SetDataToState()
	{
		D_BFAttackState.zFarDistance= zFarDistance;
		D_BFAttackState.flooringTiming= flooringTiming;
		D_BFAttackState.atkTiming= atkTiming;
		D_BFAttackState.deActiveTiming= deActiveTiming;
		D_BFAttackState.attackSpeed= attackSpeed;
	}
}
