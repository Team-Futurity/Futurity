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
	[SerializeField] protected UnityEvent resetEvent;

	[SerializeField] private TrapData trapData;

	private bool isStay = false;
	private float cooltime = .0f;

	private const int layerMask = (1 << 9);

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

	private void Awake()
	{
		startEvent.AddListener(SearchAround);
		startEvent.AddListener(ActiveTrap);
	}

	private void Update()
	{
		if(isStay)
		{
			cooltime += Time.deltaTime;

			if(cooltime >= trapData.cooldowns)
			{
				cooltime = .0f;
				isStay = false;

				resetEvent?.Invoke();
			}
		}
	}


	protected abstract void ActiveTrap();
	private void ActiveBefore()
	{
		isStay = true;
	}

	public override void Hit(UnitBase attacker, float damage)
	{
		if(isStay)
		{
			FDebug.Log("Trap Cooltime");
			return;
		}

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

	private void OnTriggerStay(Collider coll)
	{
		if (isStay || trapData.condition != 1)
		{
			return;
		}

		if (coll.CompareTag("Player"))
		{
			Active();
		}
	}

	private void Active()
	{
		startEvent?.Invoke();

		endEvent?.Invoke();

		// ������ ��ȸ���� �ƴ� ���.
		if(trapData.type != 3)
		{
			ActiveBefore();
		}
		else
		{
			// ��ȸ���� ��� ���� or ETC
		}
	}

	private void SearchAround()
	{
		//var objectList = Physics2D.OverlapCircleAll(transform.position, trapData.range);
		var objectList = Physics.OverlapSphere(transform.position, trapData.range, layerMask);

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
