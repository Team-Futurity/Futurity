using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrowdData", menuName = "Crowd/CrowdData", order = 0)]
public class CrowdData : ScriptableObject
{
	[field: SerializeField] public CrowdName CrowdName { get; private set; }
	
	[field: SerializeField] public string CrowdDescript { get; private set; }

	[field: SerializeField] public CrowdType CrowdType { get; private set; }

	[field: SerializeField] public float CrowdActiveTime { get; private set; }

	[field: SerializeField] public OriginStatus BuffStatus { get; private set; }
}
