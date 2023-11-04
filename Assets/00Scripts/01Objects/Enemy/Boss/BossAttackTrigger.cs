using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
	[SerializeField] private BossController bc;
	[SerializeField] private GameObject objParent;
	public Transform type3StartPos;

	[Space(8)]
	[Header("Attack Collider")]
	public GameObject type0Collider;
	public GameObject type1Collider;
	public GameObject type2Collider;
	public List<GameObject> type2ExtraColliders;
	public GameObject type3Collider;
	public List<GameObject> type4Colliders;
	public SpawnerManager type5Manager;
	public List<GameObject> type6Colliders;

	public BossAttackTrigger()
	{
		bc.attackTrigger = this;
		this.transform.SetParent(objParent.transform, true);
		AttackSetting();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			DamageInfo info = new DamageInfo(bc.bossData, bc.target, bc.curAttackData.extraAttackPoint);
			bc.bossData.Attack(info);
		}
	}

	#region Active/DeActive Attack List methods
	public void AttackSetting()
	{
		type0Collider.SetActive(false);
		type1Collider.SetActive(false);
		type2Collider.SetActive(false);
		AttackListSetting(type2ExtraColliders);
		type3Collider.SetActive(false); ;
		AttackListSetting(type4Colliders);
		AttackListSetting(type6Colliders);
	}

	public void AttackListSetting(List<GameObject> list)
	{
		if (list.Count > 0)
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetActive(false);
			}
	}

	public void ActiveListAttacks(List<GameObject> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetActive(true);
		}
	}

	public void DeActiveListAttacks(List<GameObject> list)
	{
		bc.listEffectData.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetActive(false);
		}
	}
	#endregion
}
