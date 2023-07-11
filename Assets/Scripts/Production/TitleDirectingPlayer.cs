using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleDirectingPlayer : MonoBehaviour
{
	public UnityEvent anyKeyInputEvent;

	public void Update()
	{
		if(Input.anyKeyDown)
		{
			anyKeyInputEvent?.Invoke();
		}
	}
}
