using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CutSceneStruct
{
	public List<GameObject> chapterScene = new List<GameObject>();
	public List<GameObject> publicScene = new List<GameObject>();
	public List<GameObject> activeScene = new List<GameObject>();
}

[Serializable]
public class SerializationDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField] private List<TKey> keys = new List<TKey>();
	[SerializeField] private List<TValue> values = new List<TValue>();

	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();

		foreach (KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		Clear();

		for (int i = 0; i < Mathf.Min(keys.Count, values.Count); ++i)
		{
			Add(keys[i], values[i]);
		}
	}
	
	public TValue GetValue(TKey key)
	{
		if (TryGetValue(key, out TValue result) == true)
		{
			return result;
		}
		
		FDebug.LogError($"{key} 가 존재하지 않습니다.");
		return default;
	}
}