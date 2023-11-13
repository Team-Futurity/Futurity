using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrap : TrapBehaviour
{
	private Vector3 startPos;
	private Vector3 endPos;

	[SerializeField] private AnimationCurve upCurve;
	[SerializeField] private AnimationCurve downCurve;

	[SerializeField] private float activeTime = .0f;
	private float timer;

	public override void ActiveTrap(List<UnitBase> units)
	{
		trapStart?.Invoke();
		
		foreach (var unit in units)
		{
			var check = CheckPlayerTag(unit.tag);

			if (check)
			{
				ProceedWithPlayer(unit);
			}
			else
			{
				ProceedWithMonster(unit);
			}
		}
	}

	public override void SetData()
	{
		base.SetData();
		timer = .0f;
	}
	
	private bool CheckPlayerTag(string unitTag)
	{
		return unitTag.Equals("Player");
	}

	private void ProceedWithPlayer(UnitBase unit)
	{
		startPos = unit.gameObject.transform.position;
		endPos = startPos;

		endPos.y += 5f;
		
		StartCoroutine(ActivePlayerEffect(unit));
	}

	private void ProceedWithMonster(UnitBase unit)
	{
		DamageInfo info = new DamageInfo(trapUnit, unit, 1);
		info.SetDamage(0);
		unit.Hit(info);
		//buffProvider.SetBuff(unit, 1001);
	}

	private IEnumerator ActivePlayerEffect(UnitBase unit)
	{
		var unitObj = unit.gameObject;

		//buffProvider.SetBuff(unit, 1001);

		while (timer <= activeTime)
		{
			timer += Time.deltaTime;
			yield return null;
			unitObj.transform.position = Vector3.Lerp(startPos, endPos, upCurve.Evaluate(timer / activeTime));
		}

		timer = .0f;
		
		while (timer <= activeTime)
		{
			timer += Time.deltaTime;
			yield return null;

			var downCurveTimer = downCurve.Evaluate(timer / activeTime);
			
			unitObj.transform.position = Vector3.Lerp(endPos, startPos, downCurveTimer);
		}

		//unit.Hit(trapUnit, 0);

		trapEnd?.Invoke();
	}
	
}