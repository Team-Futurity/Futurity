using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class T6_AttackProcess : MonoBehaviour
{
	private List<FlooringAttackProcess> attackProcessBase = new List<FlooringAttackProcess>();
	private float attackSpeed;
	private int maxCount;
	private int curCount;
	private float maxRandomDistance;

	private bool isEnable = false;


	public void T6Setting(EffectActiveData floorEffect, EffectActiveData attackEffect, SphereCollider collider, float flooringT, float atkEffectT, float attackT, float deActiveT, float attackSpeed, int maxCount, float maxRandomDistance, EnemyController ec = null, BossController bc = null)
	{
		for(int i = 0;i < maxCount; i++)
		{
			attackProcessBase.Add(new FlooringAttackProcess());
			attackProcessBase[i].Setting(floorEffect, attackEffect, collider, flooringT, atkEffectT, attackT, deActiveT, ec, bc);
		}
		this.attackSpeed = attackSpeed;
		this.maxCount = maxCount;
		this.maxRandomDistance = maxRandomDistance;
		curCount = 0;
	}

	public void StartProcess()
	{
		isEnable = true;
	}


	private void Start()
	{

	}

	private IEnumerator T6Attack()
	{
		while(curCount != maxCount - 1)
		{
			attackProcessBase[curCount].StartProcess();

			yield return ;
		}
		yield return null;
	}
}
