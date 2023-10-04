using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
	Capsule,
	Box
}

[System.Serializable]
public class ColliderData
{
	public ColliderType colliderType;
	public ColliderBase colliderScript;
	[HideInInspector] public Collider collider;
}