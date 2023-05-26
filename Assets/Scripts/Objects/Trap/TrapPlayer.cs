using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlayer : UnitBase
{
	public TrapBehaviour trapBehaviour;
	[field: SerializeField] public TrapData TrapData { get; private set; }
	
	private bool isActive;

	private void Awake()
	{
		ResetTrap();
	}

	private void FixedUpdate()
	{
		if(isActive)
		{
			SearchAround();
		}
	}

	public void SearchAround()
	{
		// �ش� �޼��忡�� ���� ���� ��ŭ�� �ֺ��� �˻��Ѵ�.
		// ���� �˻� ���θ� Ȯ���� ��.
	}

	private void ActiveTrap(UnitBase[] units)
	{
		trapBehaviour.ActiveTrap(units);
	}

	private void ResetTrap()
	{
		isActive = true;
		trapBehaviour.SetData();
	}
	
	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
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

	protected override float GetDamage(float damageValue)
	{
		return .0f;
	}

	public override void Attack(UnitBase target)
	{
	}

	#endregion

}
