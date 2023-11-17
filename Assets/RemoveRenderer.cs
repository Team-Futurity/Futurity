using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRenderer : MonoBehaviour
{
	public float timer;
	
	public void StartRemoveProcess()
	{
		StartCoroutine(StartProcess());
	}

	private IEnumerator StartProcess()
	{
		yield return new WaitForSeconds(timer);
		
		Destroy(gameObject);
	}
}
