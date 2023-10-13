using UnityEngine;
using FMODUnity;

[System.Serializable]
public class CSAttackAssetSaveData
{
	// Part Code
	public int PartCode { get; set; }

	// Attack Effect
	public Vector3 EffectOffset { get; set; }
	public Vector3 EffectRotOffset { get; set; }
	public GameObject EffectPrefab { get; set; }
	public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	public Vector3 HitEffectOffset { get; set; }
	public Vector3 HitEffectRotOffset { get; set; }
	public GameObject HitEffectPrefab { get; set; }
	public EffectParent HitEffectParent { get; set; }

	// Attack Sound
	public EventReference AttackSound { get; set; }
}