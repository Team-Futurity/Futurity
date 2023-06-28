using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleDirectingBehaviour : MonoBehaviour
{
	public UnityEvent anyKeyInputEvent;

	public void Update()
	{
		if(Input.anyKeyDown)
		{
			Debug.Log("TEST");
			anyKeyInputEvent?.Invoke();
		}
	}
}
