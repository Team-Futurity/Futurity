using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransitionProtocol
{
	public int transitionID;
	public float transitionColliderRadius;
	public float attackDamage;
	public int transitionCount;
	public LayerMask targetLayer;
	public LineRenderer lineRenderer;

	public TransitionProtocol(int id, float radius, float damage, int count, LayerMask layer, LineRenderer render)
	{
		transitionID = id;
		transitionColliderRadius = radius;
		attackDamage = damage;
		transitionCount = count;
		targetLayer = layer;
		lineRenderer = render;
	}
}