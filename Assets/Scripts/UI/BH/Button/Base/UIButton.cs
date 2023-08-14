using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour
{
	[field:SerializeField] public UnityEvent activeEvent { get; private set; }

	public void Action()
	{
		activeEvent?.Invoke();
	}
}
