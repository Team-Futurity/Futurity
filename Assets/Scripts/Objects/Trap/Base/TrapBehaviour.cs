using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TrapBehaviour : UnitBase
{
	// Event 
	[SerializeField] protected UnityEvent startEvent;
	[SerializeField] protected UnityEvent endEvent;
	[SerializeField] protected UnityEvent resetEvent;

	// Data
	[SerializeField] protected TrapData trapData;

	private bool isStay = false;
	private float cooltime = .0f;

	// Unit Layer Check
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
		startEvent.AddListener(OnTrapStart);
		endEvent.AddListener(OnTrapEnd);
		
		resetEvent.AddListener(OnTrapReset);
	}

	private void Update()
	{
		// Cooltime Routine
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

	protected abstract void OnTrapStart();
	protected abstract void OnTrapEnd();
	protected abstract void OnTrapReset();
	
	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
	{
		if (isStay)
		{
			return;
		}
		
		// Trap Condition이 공격이 아닐 경우.
		if(trapData.condition != TrapCondition.ATTACK)
		{
			return;
		}

		status.SetStatus(StatusName.CURRENT_HP, -damage);
		
		if(status.GetStatus(StatusName.CURRENT_HP) <= 0)
		{
			Active();
		}
	}

	private void OnTriggerStay(Collider coll)
	{
		if (isStay)
		{
			return;
		}

		// Trap Condition이 플레이어 접근이 아닐 경우.
		if(trapData.condition != TrapCondition.IN)
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

		// Explosion은 일회용이기 때문에 해당 Reset을 통하지 않는다.
		if(trapData.type != TrapType.Explosion)
		{
			isStay = true;
		}
	}

	protected Collider[] GetAroundObject()
	{
		var objectList = Physics.OverlapSphere(transform.position, trapData.range, layerMask);
		return (objectList.Length > 0) ? objectList : null;
	}
}
