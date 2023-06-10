using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffData", order = 0)]
public class BuffData : ScriptableObject
{
	[field: SerializeField] public int BuffCode { get; private set; }

	[field: SerializeField] public string BuffDescript { get; private set; }

	[field: SerializeField] public BuffType BuffType { get; private set; }

	[field: SerializeField] public BuffName BuffName { get; private set; }

	[field: SerializeField] public float BuffActiveTime { get; private set; }

	[field: SerializeField] public GameObject BuffEffect { get; private set; }
	
	[field: SerializeField] public OriginStatus BuffStatus { get; private set; }
}
