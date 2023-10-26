using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyState.Hitted)]
public class EnemyHittedState : StateBase
{
	private float hitColorChangeTime = 0.2f;
	private float hitMaxTime = 0.4f;
	private bool isColorChanged = false;
	private Color defaultColor = new Color(1, 1, 1, 0f);

	public override void Begin(EnemyController unit)
	{
		curTime = 0;
		unit.animator.SetTrigger(unit.hitAnimParam);
		unit.copyUMat.SetColor(unit.matColorProperty, unit.damagedColor);

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
			unit.DelayChangeState(curTime, hitMaxTime, unit, unit.UnitChaseState());

		//Death event
		if (unit.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			if (!unit.IsCurrentState(EnemyState.Death))
			{
				unit.ChangeState(EnemyState.Death);
			}
		}
	}

	public override void End(EnemyController unit)
	{
		unit.rigid.velocity = Vector3.zero;
		isColorChanged = false;
		unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
	}
}
