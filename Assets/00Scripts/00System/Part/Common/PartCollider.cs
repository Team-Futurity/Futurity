using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartCollider
{
	public static Collider[] DrawCircleCollider(Vector3 createPos, float radius, LayerMask targetLayer)
	{
		var coll = Physics.OverlapSphere(createPos, radius, targetLayer);
		return coll;
	}

	public static Collider[] DrawRectCollider(Vector3 startPos, Vector3 halfSize, LayerMask targetLayer)
	{
		var coll = Physics.OverlapBox(startPos, halfSize, Quaternion.identity, targetLayer);
		return coll;
	}
}
