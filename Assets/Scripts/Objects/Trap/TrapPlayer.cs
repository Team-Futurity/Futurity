using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BuffSystem))]
[RequireComponent(typeof(StatusManager))]
public class TrapPlayer : UnitBase
{
	private List<UnitBase> detectList;
	private bool isActive = true;
	
	[Header("Æ®·¦")]
	private TrapBehaviour trapBehaviour;
	[field: SerializeField] public TrapData TrapData { get; private set; }
	[SerializeField] private LayerMask searchLayer;

	private WaitForSeconds waitTime;

	private void Awake()
	{
		detectList = new List<UnitBase>();
		TryGetComponent(out trapBehaviour);
		waitTime = new WaitForSeconds(TrapData.TrapCooldowns);
		
		ResetTrap();
		
		trapBehaviour.trapEnd.AddListener(ResetTrap);
		trapBehaviour.trapEnd.AddListener(SetCooldowns);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			ActiveTrap(detectList);
		}
	}

	private void FixedUpdate()
	{
		if(isActive && TrapData.TrapCondition == TrapCondition.IN)
		{
			SearchAround();
		}
	}
	
	public void SearchAround()
	{
		var allUnit = Physics.OverlapSphere(transform.position, TrapData.TrapRange, searchLayer);
		
		if (allUnit.Length > 0)
		{
			isActive = false;
		}
		else
		{
			return;
		}
		
		foreach (var unit in allUnit)
		{
			detectList.Add(unit.GetComponent<UnitBase>());
		}
		
		ActiveTrap(detectList);
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
	{
		if (isActive && TrapData.TrapCondition == TrapCondition.ATTACK)
		{
			isActive = false;

			detectList.Add(attacker);
		}
		
		ActiveTrap(detectList);
	}

	private void SetCooldowns()
	{
		StartCoroutine(StartCooltime());
	}

	private IEnumerator StartCooltime()
	{
		yield return waitTime;
		
		isActive = true;
	}
	
	private void ActiveTrap(List<UnitBase> units)
	{
		trapBehaviour.ActiveTrap(units);
	}

	private void ResetTrap()
	{
		trapBehaviour.SetData();
		detectList.Clear();
	}
	
	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, TrapData.TrapRange);

		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, Vector3.forward);
	}
	#endif

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
