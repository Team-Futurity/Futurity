using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[FSMState((int)EnemyState.RDefaultAttack)]
public class RDefaultAttackState : EnemyAttackBaseState
{
	private float attackTime = 0.6f;
	private bool isAttackDone = false;
	private float distance = .0f;
	private Vector3 projectilePos = new Vector3(0f, 1.05f, 0f);

	private float projectileDistance = 10f;
	private GameObject rangedProjectile;

	private EffectActiveData effectActiveData = new EffectActiveData();

	public RDefaultAttackState()
	{
		effectActiveData.activationTime = EffectActivationTime.AttackReady;
		effectActiveData.target = EffectTarget.Caster;
		effectActiveData.position = new Vector3(0f, 1.1f, 0.7f);
		effectActiveData.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		if (!rangedProjectile)
			rangedProjectile = unit.GetComponentInChildren<Projectile>().gameObject;

		effectActiveData.parent = unit.gameObject;
		unit.currentEffectData = effectActiveData;
		rangedProjectile.transform.position = unit.transform.position + projectilePos;
		rangedProjectile.transform.rotation = unit.transform.rotation;

		AudioManager.Instance.PlayOneShot(unit.attackSound1, unit.transform.position);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
		distance = Vector3.Distance(unit.transform.position, rangedProjectile.transform.position);
		
		if(curTime > attackTime && !isAttackDone)
		{
			rangedProjectile.SetActive(true);
			unit.enemyData.EnableAttackTiming();
			isAttackDone = true;
		}

		if (distance > projectileDistance)
			rangedProjectile.SetActive(false);
	}

	public override void End(EnemyController unit)
	{
		base.End(unit);
		isAttackDone = false;
	}
}
