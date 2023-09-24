using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	public List<TKey> keys;
	public List<TValue> values;

	public SerializableDictionary()
	{
		keys = new List<TKey>();
		values = new List<TValue>();
		SyncInspectorFromDictionary();
	}
	public SerializableDictionary(SerializableDictionary<TKey, TValue> dictionary)
	{
		keys = new List<TKey>();
		values = new List<TValue>();

		keys.CopyTo(dictionary.keys.ToArray());
		values.CopyTo(dictionary.values.ToArray());
	}

	public new void Add(TKey key, TValue value)
	{
		base.Add(key, value);
		SyncInspectorFromDictionary();
	}

	public new void Remove(TKey key)
	{
		base.Remove(key);
		SyncInspectorFromDictionary();
	}

	public void SyncInspectorFromDictionary()
	{
		keys.Clear();
		values.Clear();

		foreach (KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key); values.Add(pair.Value);
		}
	}

	public void SyncDictionaryFromInspector()
	{
		foreach (var key in Keys)
		{
			base.Remove(key);
		}

		for (int i = 0; i < keys.Count; i++)
		{
			if (this.ContainsKey(keys[i]))
			{
				FDebug.LogError("중복된 키가 있습니다.");
				break;
			}
			base.Add(keys[i], values[i]);
		}
	}

	public void OnAfterDeserialize()
	{
		if (keys.Count == values.Count)
		{
			SyncDictionaryFromInspector();
		}
	}

	public void OnBeforeSerialize()
	{

	}
}
