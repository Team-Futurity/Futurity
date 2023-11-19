using UnityEngine;

public class FlooringAttackProcess : MonoBehaviour
{
	private float curTime = 0f;
	private EnemyController ec;
	private BossController bc;

	private float flooringTiming = 0f;
	private float atkEffectTiming = 0f;
	private float atkTiming = 0f;
	private float deActiveTiming = 0f;
	private int index = 0;

	private bool isFlooringDone = false;
	private bool isAtkDone = false;
	private bool isSystemEnable = false;

	private EffectActiveData floorEffectData = new EffectActiveData();
	private EffectActiveData attackEffectData = new EffectActiveData();
	private GameObject atkCollider;

	public void Setting(EffectActiveData floorEffect, EffectActiveData attackEffect, GameObject collider, float flooringT,float atkEffectT, float attackT, float deActiveT,int index, EnemyController ec = null, BossController bc = null)
	{
		this.floorEffectData = floorEffect;
		this.attackEffectData = attackEffect;
		this.atkCollider = collider;
		this.flooringTiming = flooringT;
		this.atkEffectTiming= atkEffectT;
		this.atkTiming = attackT;
		this.deActiveTiming = deActiveT;
		this.index = index;
		this.ec = ec;
		this.bc = bc;
	}

	public void StartProcess(bool start = true)
	{
		isSystemEnable = start;

		if (bc != null)
		{
			bc.bossData.EnableAttackTiming();
			bc.attackTrigger.type6Colliders[index].transform.SetParent(bc.transform);
		}
		else if (ec != null)
			ec.enemyData.EnableAttackTiming();
	}

	private void Update()
	{
		if (isSystemEnable)
		{
			curTime += Time.deltaTime;
			if (!isFlooringDone && curTime > flooringTiming)
			{
				ActiveEffect(floorEffectData);
				isFlooringDone = true;
			}
			else if (isFlooringDone && !isAtkDone && curTime > flooringTiming + atkTiming)
			{
				ActiveEffect(attackEffectData);
				atkCollider.SetActive(true);
				isAtkDone = true;
			}
			else if (isFlooringDone && isAtkDone && curTime > flooringTiming + atkTiming + deActiveTiming)
			{
				bc.attackTrigger.type6Colliders[index].transform.SetParent(null);
				atkCollider.SetActive(false);
				isSystemEnable = false;
			}
		}
	}

	private void ActiveEffect(EffectActiveData effectData)
	{
		EffectActiveData data = effectData;
		EffectKey key = new EffectKey();
		var particles = new ParticleActiveController();
		if (bc != null)
		{
			key = bc.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, 0);
			particles = key.EffectObject.GetComponent<ParticleActiveController>();
			AudioManager.Instance.PlayOneShot(bc.soundDataSO.GetSoundReference(bc.curState, SoundType.ATTACK_EFFECT), key.EffectObject.transform.position);
		}

		else if (ec != null)
		{ 
			key = ec.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, 0);
			particles = key.EffectObject.GetComponent<ParticleActiveController>();
		}


		if (particles != null)
		{
			particles.Initialize(bc.effectController, key);
		}
	}
}
