using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(BuffProvider))]
public abstract class TrapBehaviour : MonoBehaviour
{
	// Event 
	[SerializeField] public UnityEvent trapStart;
	[SerializeField] public UnityEvent trapEnd;
	[SerializeField] public UnityEvent trapReset;
	[field: SerializeField] public TrapData TrapData { get; private set; }

//	protected BuffProvider buffProvider;
	protected UnitBase trapUnit;

	protected void Awake()
	{
//		TryGetComponent(out buffProvider);
		TryGetComponent(out trapUnit);
	}

	public virtual void SetData()
	{
		trapReset?.Invoke();
	}

	public abstract void ActiveTrap(List<UnitBase> units);

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, TrapData.TrapRange);

		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, Vector3.forward);
	}
#endif
}
