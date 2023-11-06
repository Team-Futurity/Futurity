using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaActivePart : SpecialMoveProcessor
{
	// Common
	public Transform firstEffectPos;
	public Transform secondEffectPos;
	public Transform thirdEffectPos;

	public ObjectPoolManager<Transform> firstAttackObjectPool;
	public ObjectPoolManager<Transform> secondAttackObjectPool;
	public ObjectPoolManager<Transform> thirdAttackObjectPool;
	
	[Space(10)]
	
	// Phase 1
	public float firstMaxAngle;	
	public float firstRadius;
	public float firstDamage;
	
	[SerializeField]
	private GameObject firstAttackEffect;
	[SerializeField]
	private GameObject firstAttackEffectParent;

	// Phase 2
	public float secondMaxAngle;
	public float secondRadius;
	public float secondDamage;
	
	[SerializeField]
	private GameObject secondAttackEffect;
	[SerializeField]
	private GameObject secondAttackEffectParent;

	// Phase 3
	public float thirdMaxWdith;
	public float thirdMaxHeight;
	public float thirdDamage;
	
	[SerializeField]
	private GameObject thirdAttackEffect;
	[SerializeField]
	private GameObject thirdAttackEffectParent;
	
	public override void GetPartData()
	{
		if (firstAttackObjectPool == null) firstAttackObjectPool = new ObjectPoolManager<Transform>(firstAttackEffect, firstAttackEffectParent);
		if (secondAttackObjectPool == null) secondAttackObjectPool = new ObjectPoolManager<Transform>(secondAttackEffect, secondAttackEffectParent);
		if (thirdAttackObjectPool == null) thirdAttackObjectPool = new ObjectPoolManager<Transform>(thirdAttackEffect, thirdAttackEffectParent);
	}
}
