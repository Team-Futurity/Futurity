using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColliderChanger : MonoBehaviour
{
	public ColliderData[] colliderDatas;
	private Dictionary<ColliderType, ColliderData> colliderDictionary;

	private void Start()
	{
		colliderDictionary = new Dictionary<ColliderType, ColliderData>();
		foreach(var data in colliderDatas)
		{
			if (colliderDictionary.ContainsKey(data.colliderType)) { continue; }

			Collider collider = GetColliderByColliderBase(data.colliderScript);
			if (collider == null) { FDebug.LogWarning("Collider is not Assigned", GetType()); continue; }

			collider.enabled = false;
			data.collider = collider;

			colliderDictionary.Add(data.colliderType, data);
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

	private ColliderData GetColliderWithDictionary(ColliderType type)
	{
		ColliderData colliderData = null;
		if (!colliderDictionary.TryGetValue(type, out colliderData)) { FDebug.LogWarning($"This Collider Type is not Exist : {type}", GetType()); return null; }
		if (colliderData.collider == null) { FDebug.LogWarning("Collider is Null", GetType()); return null; }

		return colliderData;
	}

	public ColliderBase GetCollider(ColliderType type)
	{
		ColliderData colliderData = null;

		if(!colliderDictionary.TryGetValue(type, out colliderData)) { FDebug.LogWarning($"This Collider Type is not Exist : {type}", GetType()); return null; }

		return colliderData.colliderScript;
	}


	public bool EnableCollider(ColliderType type, out ColliderBase collider)
	{
		ColliderData colliderToEnable = GetColliderWithDictionary(type);
		collider = colliderToEnable.colliderScript;

		if(colliderToEnable == null) { return false; }
		
		foreach(var colliderValue in colliderDictionary.Values)
		{
			colliderValue.collider.enabled = false;
		}

		colliderToEnable.collider.enabled = true;

		return true;
	}

	public bool EnableCollider(ColliderType type)
	{
		ColliderBase colliderBase;
		return EnableCollider(type, out colliderBase);
	}

	public bool DisableCollider(ColliderType type, out ColliderBase collider)
	{
		ColliderData colliderToEnable = GetColliderWithDictionary(type); 
		collider = colliderToEnable.colliderScript;

		if (colliderToEnable == null) { return false; }

		colliderToEnable.collider.enabled = false;

		return true;
	}

	public bool DisableCollider(ColliderType type)
	{
		ColliderBase colliderBase;
		return DisableCollider(type, out colliderBase);
	}

	public void DisableAllCollider()
	{
		foreach (var colliderData in colliderDictionary.Values)
		{
			colliderData.collider.enabled = false;
		}
	}
}
