using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColliderChanger : MonoBehaviour
{
	public ColliderData[] colliderDatas;
	private Dictionary<ColliderType, Collider> colliderDictionary;

	private void Awake()
	{
		colliderDictionary = new Dictionary<ColliderType, Collider>();
		foreach(var data in colliderDatas)
		{
			if (colliderDictionary.ContainsKey(data.colliderType)) { continue; }

			Collider collider = GetColliderByColliderBase(data.collider);
			if (collider == null) { FDebug.LogWarning("Collider is not Assigned", GetType()); continue; }


			colliderDictionary.Add(data.colliderType, collider);
		}
	}

	private Collider GetColliderByColliderBase(ColliderBase data)
	{
		if(data is TruncatedCapsuleCollider capsuleCollider)
		{
			return capsuleCollider.ColliderReference;
		}
		else if (data is TruncatedBoxCollider boxCollider)
		{
			return boxCollider.ColliderReference;
		}

		return null;
	}

	private Collider GetColliderWithDictionary(ColliderType type)
	{
		Collider collider = null;
		if (!colliderDictionary.TryGetValue(type, out collider)) { FDebug.LogWarning($"This Collider Type not Exist : {type}", GetType()); return null; }
		if (collider == null) { FDebug.LogWarning("Collider is Null", GetType()); return null; }

		return collider;
	}


	public bool EnableCollider(ColliderType type)
	{
		Collider colliderToEnable = GetColliderWithDictionary(type);

		if(colliderToEnable == null) { return false; }
		
		foreach(var collider in colliderDictionary.Values)
		{
			collider.enabled = false;
		}

		colliderToEnable.enabled = true;

		return true;
	}

	public bool DisableCollider(ColliderType type)
	{
		Collider colliderToEnable = GetColliderWithDictionary(type);

		if (colliderToEnable == null) { return false; }

		colliderToEnable.enabled = false;

		return true;
	}

	public void DisableAllCollider()
	{
		foreach (var collider in colliderDictionary.Values)
		{
			collider.enabled = false;
		}
	}
}
