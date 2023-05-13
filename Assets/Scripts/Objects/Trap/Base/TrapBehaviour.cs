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
	[SerializeField] protected UnityEvent resetEvent;

	[SerializeField] protected TrapData trapData;

	private bool isStay = false;
	private float cooltime = .0f;

	private const int layerMask = (1 << 9);

	// Buff System 추가 예정

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
	private void ActiveBeforeTrap()
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

		if(trapData.type != 3)
		{
			ActiveBeforeTrap();
		}
		else
		{

		}
	}

	protected Collider[] SearchAround()
	{
		//var objectList = Physics2D.OverlapCircleAll(transform.position, trapData.range);
		var objectList = Physics.OverlapSphere(transform.position, trapData.range, layerMask);

		if (objectList.Length is <= 0)
			return null;

		return objectList;
	}

}
