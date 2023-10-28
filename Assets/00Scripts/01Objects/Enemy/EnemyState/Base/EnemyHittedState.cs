using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyState.Hitted)]
public class EnemyHittedState : StateBase
{
	private float hitColorChangeTime = 0.2f;

	private bool isColorChanged = false;
	private Color defaultColor = new Color(1, 1, 1, 0f);

	Vector3 direction;

	public override void Begin(EnemyController unit)
	{
		curTime = 0;
		
		unit.copyUMat.SetColor(unit.matColorProperty, unit.damagedColor);
		PrintAnimation(unit);
		unit.enemyData.StopAnimation(unit.stopFrameCount, unit.skipFrameCountBeforeStop);

		if(unit.currentEffectKey != null)
			unit.effectController.RemoveEffect(unit.currentEffectKey);

		AudioManager.Instance.PlayOneShot(unit.hitSound, unit.transform.position);
	}
	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if(!isColorChanged)
			if(curTime > hitColorChangeTime)
			{
				unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
				isColorChanged = true;
			}

		if (unit.ThisEnemyType == EnemyType.TutorialDummy)
			unit.DelayChangeState(curTime, 0.5f, unit, EnemyState.TutorialIdle);
		else
			unit.DelayChangeState(curTime, unit.hitDelay, unit, unit.UnitChaseState());

		DeathEvent(unit);
	}

	public override void End(EnemyController unit)
	{
		unit.rigid.velocity = Vector3.zero;
		isColorChanged = false;
		unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
	}

	public void DeathEvent(EnemyController unit)
	{
		if (unit.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			if (!unit.IsCurrentState(EnemyState.Death))
			{
				unit.ChangeState(EnemyState.Death);
			}
		}
	}

	public void PrintAnimation(EnemyController unit)
	{
		direction = unit.transform.position - unit.target.transform.position;

		if (direction.x > 0)
		{
			if (unit.transform.eulerAngles.y > 0 && unit.transform.eulerAngles.y < 180)
				unit.animator.SetTrigger(unit.hitBAnimParam);
			else if (unit.transform.eulerAngles.y > 180 && unit.transform.eulerAngles.y < 360)
				unit.animator.SetTrigger(unit.hitFAnimParam);
		}
		else if (direction.x < 0)
		{
			if (unit.transform.eulerAngles.y > 0 && unit.transform.eulerAngles.y < 180)
				unit.animator.SetTrigger(unit.hitFAnimParam);
			else if (unit.transform.eulerAngles.y > 180 && unit.transform.eulerAngles.y < 360)
				unit.animator.SetTrigger(unit.hitBAnimParam);
		}
	}
}
