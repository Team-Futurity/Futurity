using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliders : MonoBehaviour
{
	[SerializeField] private BossController bc;
	[SerializeField] private GameObject objParent;
	public Transform type3StartPos;
	[SerializeField] private float type6Radius = 7f;

	[Space(8)]
	[Header("Attack Collider")]
	public GameObject type0Collider;
	public GameObject type1Collider;
	public GameObject type2Collider;
	public List<GameObject> type2ExtraColliders;
	public List<GameObject> type3Colliders;
	public List<GameObject> type4Colliders;
	public SpawnerManager type5Manager;
	public List<GameObject> type6Colliders;
	public List<GameObject> type6ExtraColliders;


	private void Awake()
	{
		if(bc.attackTrigger == null)
			bc.attackTrigger = new AttackColliders();
		bc.attackTrigger = this;
		this.transform.SetParent(objParent.transform, true);
		AttackSetting();
	}

	#region Active/DeActive Attack List methods
	public void AttackSetting()
	{
		type0Collider.transform.SetParent(bc.transform, true);
		type1Collider.transform.SetParent(bc.transform, true);
		type2Collider.transform.SetParent(bc.transform, true);
		foreach(GameObject o in type2ExtraColliders)
			o.transform.SetParent(type2Collider.transform, true);
		type3StartPos.transform.SetParent(null, true);
		type0Collider.SetActive(false);
		type1Collider.SetActive(false);
		type2Collider.SetActive(false);
		AttackListSetting(type2ExtraColliders);
		AttackListSetting(type3Colliders);
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
			list[i].transform.SetParent(bc.transform, true);
			list[i].SetActive(true);
		}
	}

	public void DeActiveListAttacks(List<GameObject> list)
	{
		bc.listEffectData.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			list[i].transform.SetParent(this.transform, true);
			list[i].SetActive(false);
		}
	}

	public void SetRandomVector(BossController unit)
	{
		Vector2 randomPos;
		for (int i = 0; i < unit.attackTrigger.type6Colliders.Count; i++)
		{
			randomPos = Random.insideUnitCircle * type6Radius;
			unit.attackTrigger.type6Colliders[i].transform.position = new Vector3(randomPos.x, 0, randomPos.y) + unit.transform.position;
		}
	}
	#endregion
}
