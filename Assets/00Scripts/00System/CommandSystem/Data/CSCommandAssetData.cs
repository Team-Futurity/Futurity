using UnityEngine;
using FMODUnity;
using System;

[Serializable]
public class CSCommandAssetData
{
	// Part Code
	[field: SerializeField] public int PartCode { get; set; }

	// Attack Effect
	[field: SerializeField] public Vector3 EffectOffset { get; set; }
	[field: SerializeField] public Vector3 EffectRotOffset { get; set; }
	[field: SerializeField] public GameObject EffectPrefab { get; set; }
	[field: SerializeField] public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	[field: SerializeField] public Vector3 HitEffectOffset { get; set; }
	[field: SerializeField] public Vector3 HitEffectRotOffset { get; set; }
	[field: SerializeField] public GameObject HitEffectPrefab { get; set; }
	[field: SerializeField] public EffectParent HitEffectParent { get; set; }

	// Attack Sound
	[field: SerializeField] public EventReference AttackSound { get; set; }
}