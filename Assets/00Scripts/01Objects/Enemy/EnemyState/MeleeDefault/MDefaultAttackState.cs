using UnityEngine;

[FSMState((int)EnemyState.MDefaultAttack)]
public class MDefaultAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();

	public MDefaultAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
		atk1.position = new Vector3(0.047f, 0.953f, 0.03f);
		atk1.rotation = Quaternion.Euler(new Vector3(7.567f, -0.62f, 22.347f));
	}

	public override void Begin(EnemyController unit)
	{

		base.Begin(unit);
		atk1.parent = unit.gameObject;
		unit.currentEffectData = atk1;
	}
}
