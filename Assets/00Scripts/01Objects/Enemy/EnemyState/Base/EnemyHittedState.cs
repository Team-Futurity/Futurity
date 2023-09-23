using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	private bool isColorChanged = false;
	private float curTime;
	private Color defaultColor = new Color(1, 1, 1, 0f);

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Hit Begin");
		curTime = 0;

		//unit.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

		unit.animator.SetTrigger(unit.hitAnimParam);
		unit.copyUMat.SetColor(unit.matColorProperty, unit.damagedColor);

		AudioManager.instance.PlayOneShot(unit.hitSound, unit.transform.position);
	}
	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if(!isColorChanged)
			if(curTime > unit.hitColorChangeTime)
			{
				unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
				isColorChanged = true;
			}

		if (unit.isTutorialDummy)
			unit.DelayChangeState(curTime, 0.5f, unit, EnemyController.EnemyState.TutorialIdle);
		else
			unit.DelayChangeState(curTime, unit.hitMaxTime, unit, unit.UnitChaseState());

		//Death event
		if (unit.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			if (!unit.IsCurrentState(EnemyState.Death))
			{
				unit.ChangeState(EnemyState.Death);
			}
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Hit End");

		//unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.rigid.velocity = Vector3.zero;
		isColorChanged = false;
		unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
