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

	[SerializeField] private TrapData trapData;

	private bool isStay = false;
	private float cooltime = .0f;

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
	private void ResetTrap()
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

	private void OnTriggerEnter(Collider collider)
	{
		if (isStay)
		{
			FDebug.Log("Trap Cooltime");
			return;
		}

		if (trapData.condition != 1)
		{
			return;
		}

		if (collider.CompareTag("Player"))
		{
			Active();
		}
	}

	private void Active()
	{
		startEvent?.Invoke();

		endEvent?.Invoke();

		// 함정이 일회용이 아닐 경우.
		if(trapData.type != 3)
		{
			ResetTrap();
		}
		else
		{
			// 일회용일 경우 삭제 or ETC
		}
	}

	private void SearchAround()
	{
		var objectList = Physics2D.OverlapCircleAll(transform.position, trapData.range);

		// 탐색된 Object가 없다면
		if (objectList.Length is <= 0)
			return;

		// 탐색된 Object List를 가져와서 처리해준다.
		foreach(var obj in objectList)
		{
			// 데미지를 주며
			obj.GetComponent<UnitBase>().Hit(this, trapData.damage);

			// 디버프를 할당한다. 단, 타입이 단순 데미지가 아니어야 함.
			if(trapData.debuff == 1)
			{
				// 디버프 할당. -> BuffSystem이 PR이 되지 않음.
			}
		}
		
	}

}
