using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : UnitBase
{
	// 발동 조건에 따라서 함정이 발동됨. -> 현재까지는 2개
	// 오브젝트와 충돌하면 동작을 취함
	[SerializeField] protected UnityEvent startEvent;
	[SerializeField] protected UnityEvent endEvent;

	[SerializeField]
	protected TrapData trapData;

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

	protected override float GetDamage()
	{
		return .0f;
	}

	public override void Attack(UnitBase target)
	{
	}

	#endregion


	protected abstract void ActiveTrap();
	protected abstract void ResetTrap();

	public override void Hit(UnitBase attacker, float damage)
	{
		// Trap Condition이 공격이 아닐 경우, currentHP가 0보다 작을 경우.
		if(trapData.condition != 2 || status.currentHp is <= 0)
		{
			return;
		}

		status.currentHp -= damage;
		
		if(status.currentHp is <= 0)
		{
			Active();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(trapData.condition != 1)
		{
			return;
		}

		// Monster 혹은 Player일 경우.
		if (collision.collider.CompareTag("Monster") || collision.collider.CompareTag("Player"))
		{
			Active();
		}
	}

	private void Active()
	{
		startEvent?.Invoke();

		ActiveTrap();

		// 일회용이 아닐 경우
		if(trapData.type != 3)
		{
			ResetTrap();
		}
	}

}
