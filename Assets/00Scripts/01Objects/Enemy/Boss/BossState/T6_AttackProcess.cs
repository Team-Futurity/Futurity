using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class T6_AttackProcess : MonoBehaviour
{
	private List<FlooringAttackProcess> attackProcesss;
	private int maxCount;
	private WaitForSeconds attackWFS;

	public void T6Setting(List<EffectActiveData> floorEffects, List<EffectActiveData> attackEffects, List<GameObject> colliders, float flooringT, float atkEffectT, float attackT, float deActiveT, float attackSpeed, EnemyController ec = null, BossController bc = null)
	{
		this.maxCount = attackEffects.Count;
		attackWFS = new WaitForSeconds(attackSpeed);
		attackProcesss = new List<FlooringAttackProcess>();

		for (int i = 0;i < attackEffects.Count; i++)
		{
			GameObject obj = new GameObject("T6_Process" + i);
			obj.AddComponent<FlooringAttackProcess>();
			attackProcesss.Add(obj.GetComponent<FlooringAttackProcess>());
			attackProcesss[i].transform.SetParent(bc.attackTrigger.objParent.transform);
			attackProcesss[i].Setting(floorEffects[i], attackEffects[i], colliders[i], flooringT, atkEffectT, attackT, deActiveT,i, ec, bc);
		}
	}

	public void StartProcess()
	{
		StopCoroutine(ActiveProcess());
		StartCoroutine(ActiveProcess());
	}

	private IEnumerator ActiveProcess()
	{
		for(int i = 0; i<maxCount; i++)
		{
			attackProcesss[i].StartProcess();
			yield return attackWFS;
		}

		yield break;
	}
}
