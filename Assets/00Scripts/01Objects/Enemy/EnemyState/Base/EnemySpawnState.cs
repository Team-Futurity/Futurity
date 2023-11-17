using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerController;

[FSMState((int)EnemyState.Spawn)]
public class EnemySpawnState : StateBase
{
	private float maxSpawningTime = 1f;
	private string copyDMatProperty = "_distortion";

	public override void Begin(EnemyController unit)
	{
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();
		unit.navMesh.enabled = true;

		if (unit.atkCollider != null)
			unit.atkCollider.enabled = false;

		if (unit.ThisEnemyType == EnemyType.D_LF)
			unit.SettingProjectile();

		unit.skinnedMeshRenderer.materials = new Material[1] { unit.copyDMat };
		unit.copyDMat.SetFloat(copyDMatProperty, 0f);
		unit.skinnedMeshRenderer.gameObject.layer = 7;


		ActiveSpawnEffect(unit);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, maxSpawningTime, unit, EnemyState.Idle);

		if (unit.copyDMat.GetFloat(copyDMatProperty) < 1f)
			unit.copyDMat.SetFloat(copyDMatProperty, curTime);
	}
	public override void End(EnemyController unit)
	{
		unit.enemyCollider.enabled = true;
		unit.chaseRange.enabled = true;
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.rigid.velocity = Vector3.zero;

		unit.copyDMat.SetFloat(copyDMatProperty, 1.0f);

		unit.skinnedMeshRenderer.materials = new Material [2] { unit.material, unit.copyUMat };
		unit.skinnedMeshRenderer.gameObject.layer = 0;
	}

	public void ActiveSpawnEffect(EnemyController unit)
	{
		EffectActiveData data = new EffectActiveData();

		data.activationTime = EffectActivationTime.Spawn;
		data.target = EffectTarget.Ground;
		data.index = 0;
		data.parent = null;
		data.position = unit.transform.position + unit.spawnEffectPos;
		data.rotation = Quaternion.Euler(new Vector3(-90f, 0, 0));

		EffectKey key = unit.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, 0, true);

		key.EffectObject.transform.localScale = unit.spawnEffectScale;

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(unit.effectController, key);
		}
	}
}
