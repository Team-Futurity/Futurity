using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransitionProtocol
{
	public int transitionID;
	public float transitionColliderRadius;
	public int attackDamage;
	public int transitionCount;
	public LayerMask targetLayer;

	public TransitionProtocol(int id, float radius, int damage, int count, LayerMask layer)
	{
		transitionID = id;
		transitionColliderRadius = radius;
		attackDamage = damage;
		transitionCount = count;
		targetLayer = layer;
	}
}