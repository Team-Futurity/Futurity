using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffData", order = 0)]
public class BuffData : ScriptableObject
{
	[field: SerializeField] public string BuffDescript { get; private set; }

	[field: SerializeField] public BuffTypeList BuffType { get; private set; }

	[field: SerializeField] public float BuffActiveTime { get; private set; }

	[field: SerializeField] public GameObject BuffEffect { get; private set; }
}
