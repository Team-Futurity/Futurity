using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack)]
public class MDefaultAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();

	public MDefaultAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack begin");
		base.Begin(unit);
		atk1.parent = unit.gameObject;
		unit.currentEffectData.position = new Vector3(0.047f, 0.953f, 0.03f);
		unit.currentEffectData.rotation = Quaternion.Euler(new Vector3(7.567f, -0.62f, 22.347f));
		unit.currentEffectData = atk1;
		unit.navMesh.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, unit.UnitChaseState());
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack End");

		base.End(unit);

		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		//FDebug.Log("MDefault Attack Trigger");
		base.OnTriggerEnter(unit, other);
	}
}
