using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : MonoBehaviour
{
	// Event 
	[SerializeField] public UnityEvent trapStart;
	[SerializeField] public UnityEvent trapEnd;
	[SerializeField] public UnityEvent trapReset;

	public virtual void SetData()
	{
		trapReset?.Invoke();
	}

	public virtual void ActiveTrap(List<UnitBase> units)
	{
	}
}
