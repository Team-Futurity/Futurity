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
	[field: SerializeField] public TrapData TrapData { get; private set; }

	protected UnitBase trapUnit;
	protected BuffSystem buffSystem;
	protected StatusManager statusManager;

	protected void Awake()
	{
		TryGetComponent(out trapUnit);
		TryGetComponent(out buffSystem);
		TryGetComponent(out statusManager);
	}

	public virtual void SetData()
	{
		trapReset?.Invoke();
	}

	public abstract void ActiveTrap(List<UnitBase> units);
}
