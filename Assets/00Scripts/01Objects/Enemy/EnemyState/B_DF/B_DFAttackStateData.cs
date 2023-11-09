using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateData", menuName = "ScriptableObject/Unit/B_DFStateData")]
public class B_DFAttackStateData : StateData
{
	public List<SphereCollider> attackColliders;

	public override void SetDataToState()
	{
		
	}
}
