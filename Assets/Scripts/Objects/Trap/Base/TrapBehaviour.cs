using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : ScriptableObject
{
	// Event 
	[SerializeField] protected UnityEvent trapStart;
	[SerializeField] protected UnityEvent trapEnd;
	[SerializeField] protected UnityEvent trapReset;

	
	
	public virtual void SetData()
	{
		trapReset?.Invoke();
	}

	public abstract void ActiveTrap(UnitBase[] units);

	protected virtual void StartTrap(UnitBase[] units)
	{
		trapStart?.Invoke();
	}

	protected virtual void EndTrap(UnitBase[] units)
	{
		trapEnd?.Invoke();
	}
}
