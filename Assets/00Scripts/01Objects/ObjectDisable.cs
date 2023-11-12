using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisable : MonoBehaviour
{
	[SerializeField, ReadOnly(false)] private List<GameObject> disableObject = new List<GameObject>();

	private void Start()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			disableObject.Add(transform.GetChild(i).gameObject);
		}
	}

	public void SetActiveObject(bool active) => disableObject.ForEach(x => x.SetActive(active));
}
