

using UnityEngine;

public class BasicActivePart : SpecialMoveProcessor
{
	public float minRange;  // cm 단위 (0.01unit)
	public float maxRange;	// cm 단위 (0.01unit)
	public float damage;
	public float duration;
	public int buffCode;

	public Transform chargeEffectPos;
	public Transform explosionEffectPos;
	public Transform landingEffectPos;

	[SerializeField] private GameObject chargeEffect;
	[SerializeField] private GameObject explosionEffect;
	[SerializeField] private GameObject landingEffect;
	[SerializeField] private GameObject chargeEffectParent;
	[SerializeField] private GameObject worldEffectParent;

	public ObjectPoolManager<Transform> chargeEffectObjectPool;
	public ObjectPoolManager<Transform> explosionEffectObjectPool;
	public ObjectPoolManager<Transform> landingEffectObjectPool;

	public override void GetPartData()
	{
		if(chargeEffectObjectPool == null) { chargeEffectObjectPool = new ObjectPoolManager<Transform>(chargeEffect, chargeEffectParent); }
		if(explosionEffectObjectPool == null) { explosionEffectObjectPool = new ObjectPoolManager<Transform>(explosionEffect, worldEffectParent); }
		if(landingEffectObjectPool == null) { landingEffectObjectPool = new ObjectPoolManager<Transform>(landingEffect, worldEffectParent); }
		return;
	}
}
