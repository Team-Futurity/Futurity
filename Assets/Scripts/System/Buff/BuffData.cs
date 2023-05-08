using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffData", order = 0)]
public abstract class BuffData : ScriptableObject
{
	[field: SerializeField] public string BuffName { get; private set; }

	[field: SerializeField] public BuffTypeList BuffType { get; private set; }

	[field: SerializeField] public float BuffActiveTime { get; private set; }

	[field: SerializeField] public GameObject BuffEffect { get; private set; }

	[field: SerializeField] public BuffBehaviour BuffBehaviour { get; private set; }

	public void Active()
	{
		BuffBehaviour.Active();
	}
	public void UnActive()
	{
		BuffBehaviour.UnActive();
	}
}
