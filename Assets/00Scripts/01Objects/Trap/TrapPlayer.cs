using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class TrapPlayer : UnitBase
{
	private List<UnitBase> objectDetectList = new List<UnitBase>();

	private TrapBehaviour behaviour;
	private TrapData trapData;

	private WaitForSeconds waitTime;

	private bool isTrapActive = true;

	[Header("∆Æ∑¶ µ•¿Ã≈Õ")]
	[SerializeField] private LayerMask searchLayer;

	private void Awake()
	{
		TryGetComponent(out behaviour);
		trapData = behaviour.TrapData;

		waitTime = new WaitForSeconds(trapData.TrapCooldowns);

		SetupTrapData();
	}

	private new void Start()
	{
		behaviour.trapEnd.AddListener(EndProceed);
	}

	private void FixedUpdate()
	{
		if (isTrapActive && trapData.TrapCondition == TrapCondition.IN)
		{
			SearchAround();
		}
	}

	public void SearchAround()
	{
		var aroundUnits = Physics.OverlapSphere(transform.position, trapData.TrapRange, searchLayer);
		
		if (aroundUnits.Length <= 0)
		{
			return;
		}

		isTrapActive = false;

		foreach (var unit in aroundUnits)
		{
			objectDetectList.Add(unit.GetComponent<UnitBase>());
		}

		ActiveTrap(objectDetectList);
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
	{
		if (isTrapActive && trapData.TrapCondition == TrapCondition.ATTACK)
		{
			SearchAround();
		}
	}

	private void EndProceed()
	{
		SetCooldowns();
		SetupTrapData();
	}

	private void SetCooldowns()
	{
		StartCoroutine(StartCooltime());
	}

	private IEnumerator StartCooltime()
	{
		yield return waitTime;
		isTrapActive = true;
	}
	
	private void ActiveTrap(List<UnitBase> units)
	{
		behaviour.ActiveTrap(units);
	}

	private void SetupTrapData()
	{
		behaviour.SetData();
		objectDetectList.Clear();
	}

	#region NotUsed
	protected override float GetAttackPoint()
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

	protected override void AttackProcess(DamageInfo damageInfo)
	{
		throw new System.NotImplementedException();
	}

	/*public override void Attack(UnitBase target)
	{
	}*/

	#endregion
}
