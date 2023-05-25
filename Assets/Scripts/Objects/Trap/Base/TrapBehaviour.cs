using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : UnitBase
{
	// Event 
	[SerializeField] protected UnityEvent OnStart;
	[SerializeField] protected UnityEvent OnStay;
	[SerializeField] protected UnityEvent OnEnd;
	[SerializeField] protected UnityEvent OnReset;

	private float cooltime = .0f;

	// Unit Layer Check
	private const int layerMask = (1 << 9);

	// Buff System 추가 예정

	public void SetData()
	{

	}

	public void ActiveTrap(UnitBase unit)
	{

	}

	public void ActiveTrap(UnitBase[] units)
	{

	}

	#region NotUsed
	protected override float GetAttakPoint()
	{
		return .0f;
	}

	protected override float GetDefensePoint()
	{
		return .0f;
	}

	protected override float GetCritical()
	{
		return .0f;
	}

	public override void Attack(UnitBase target)
	{
	}

	#endregion
}
