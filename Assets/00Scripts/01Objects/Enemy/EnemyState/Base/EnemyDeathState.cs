using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.Death)]
public class EnemyDeathState : StateBase
{
	private float deathDelay = 2.0f;

	private Color refColor = new Color(0f, 0f, 0f, 0f);

	public override void Begin(EnemyController unit)
	{
		unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.animator.SetTrigger(unit.deadAnimParam);
		unit.enemyCollider.enabled = false;
		
		unit.OnDisableEvent();
		unit.onDeathEvent?.Invoke();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if (refColor.a < 1f)
			refColor.a += curTime * 0.005f;
		unit.copyUMat.SetColor(unit.matColorProperty, refColor);

		if (curTime > deathDelay)
		{
			unit.gameObject.SetActive(false);
		}
	}
}
