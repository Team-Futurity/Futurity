using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrap : TrapBehaviour
{
	private BuffSystem buffSystem;
	private UnitBase trapUnit;

	private Vector3 startPos;
	private Vector3 endPos;

	[SerializeField] private AnimationCurve upCurve;
	[SerializeField] private AnimationCurve downCurve;

	[SerializeField] private float animTime = .0f;
	private float timer;


	private void Awake()
	{
		TryGetComponent(out buffSystem);
	}

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
		
		trapEnd?.Invoke();
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
		unit.Hit(trapUnit, 0);
		buffSystem.OnBuff(BuffNameList.STUN, unit);
	}

	private IEnumerator ActivePlayerEffect(UnitBase unit)
	{
		var unitObj = unit.gameObject;

		buffSystem.OnBuff(BuffNameList.STUN, unit);

		while (timer <= animTime)
		{
			timer += Time.deltaTime;
			yield return null;
			unitObj.transform.position = Vector3.Lerp(startPos, endPos, upCurve.Evaluate(timer / animTime));
		}

		timer = .0f;
		
		while (timer <= animTime)
		{
			timer += Time.deltaTime;
			yield return null;

			var downCurveTimer = downCurve.Evaluate(timer / animTime);
			
			unitObj.transform.position = Vector3.Lerp(endPos, startPos, downCurveTimer);
		}

		unit.Hit(trapUnit, 0);
	}
}