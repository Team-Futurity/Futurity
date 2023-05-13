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

	// Buff System �߰� ����

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

		if (collision.collider.CompareTag("Player"))
		{
			Active();
		}
	}

	private void Active()
	{
		startEvent?.Invoke();

		SearchAround();
		ActiveTrap();

		ResetTrap();

		endEvent?.Invoke();

		// ������ ��ȸ���� �ƴ� ���.
		if(trapData.type != 3)
		{
			ResetTrap();
		}
		else
		{
			// ��ȸ���� ��� ���� or ETC

		}
	}

	private void SearchAround()
	{
		var objectList = Physics2D.OverlapCircleAll(transform.position, trapData.range);

		// Ž���� Object�� ���ٸ�
		if (objectList.Length is <= 0)
			return;

		// Ž���� Object List�� �����ͼ� ó�����ش�.
		foreach(var obj in objectList)
		{
			// �������� �ָ�
			obj.GetComponent<UnitBase>().Hit(this, trapData.damage);

			// ������� �Ҵ��Ѵ�. ��, Ÿ���� �ܼ� �������� �ƴϾ�� ��.
			if(trapData.debuff == 1)
			{
				// ����� �Ҵ�. -> BuffSystem�� PR�� ���� ����.
			}
		}
		
	}

}
