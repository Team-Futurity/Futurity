using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : UnitBase
{
	// �ߵ� ���ǿ� ���� ������ �ߵ���. -> ��������� 2��
	// ������Ʈ�� �浹�ϸ� ������ ����
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
		// Trap Condition�� ������ �ƴ� ���, currentHP�� 0���� ���� ���.
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

		// Monster Ȥ�� Player�� ���.
		if (collision.collider.CompareTag("Monster") || collision.collider.CompareTag("Player"))
		{
			Active();
		}
	}

	private void Active()
	{
		startEvent?.Invoke();

		ActiveTrap();

		// ��ȸ���� �ƴ� ���
		if(trapData.type != 3)
		{
			ResetTrap();
		}
	}

}
