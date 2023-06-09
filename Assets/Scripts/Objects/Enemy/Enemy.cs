using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyController;
using static UnityEngine.GraphicsBuffer;

public class Enemy : UnitBase
{ 
	[SerializeField] private EnemyController ec;


	private void Start()
	{
		hpBar = GetComponent<EnemyHpBarController>().currentHpBar.GetComponent<GaugeBarController>();

		if (hpBar != null)
		{
			hpBar.SetGaugeFillAmount(status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue());
		}
	}

	public override void Attack(UnitBase target)
	{
		ec.isAttackSuccess = true;
		target.Hit(this, GetDamage(1));

	}

	public override void Hit(UnitBase attacker, float damage, bool isDot)
	{
		ec.ChangeState(EnemyController.EnemyState.Hitted);
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);


		if (hpBar != null)
		{
			hpBar.SetGaugeFillAmount(status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue());
			FDebug.Log($"SetGaugeFillAmount : {status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue()}");
			}
	}

	protected override float GetAttackPoint()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDamage(float attackCount)
	{
		float value = status.GetStatus(StatusType.ATTACK_POINT).GetValue() * attackCount * (1 + Random.Range(-0.1f, 0.1f));
		return value;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}

}
