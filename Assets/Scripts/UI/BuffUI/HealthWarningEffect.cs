using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthWarningEffect : MonoBehaviour
{
	[Header("심장 박동 이펙트")]
	[SerializeField] private Volume volume;
	[SerializeField] private float maxAlpha = 0.75f;
	[SerializeField] private float pulseSpeed = 2f;  // 효과의 박동 속도
	
	private float effectIntensity = 0f;
	private float curTime = 0.0f;
	private bool isEffectEnable = false;
	private bool isHit = false;
	
	private EnemyManager enemyManager;
	private Vignette vignette;
	private IEnumerator hitEffect;
	private IEnumerator pulseEffect;

	private void Start()
	{
		enemyManager = EnemyManager.Instance;
		volume.profile.TryGet<Vignette>(out vignette);
	}
	

	public void CheckPulseEffect(float hp)
	{
		if (hp <= 60 && isEffectEnable == false)
		{
			isEffectEnable = true;
			
			pulseEffect = WarningEffect();
			StartCoroutine(pulseEffect);
		}
		else if (hp > 60 && isEffectEnable == true)
		{
			isEffectEnable = false;	
		}
	}

	public void StartHitEffect(float time)
	{
		if (isHit == true || isEffectEnable == true)
		{
			return;
		}

		hitEffect = HitEffect(time);
		StartCoroutine(hitEffect);
	}
	
	public void PlayerDeath()
	{
		isEffectEnable = false;

		for (int i = enemyManager.activeEnemys.Count - 1; i >= 0; i--)
		{
			enemyManager.DeActiveManagement(enemyManager.activeEnemys[i]);
		}
		for (int i = enemyManager.activeEnemysHpBar.Count - 1; i >= 0; i--)
		{
			enemyManager.DeactiveHpBar(enemyManager.activeEnemysHpBar[i]);
		}
		
		TimelineManager.Instance.EnableCutScene(TimelineManager.ECutScene.PlayerDeathCutScene);
	}

	private IEnumerator HitEffect(float vignetteTime)
	{
		isHit = true;
		
		while (vignetteTime > curTime)
		{
			curTime += Time.deltaTime;
			vignette.intensity.value = 0 + curTime;

			yield return null;
		}
		
		vignette.intensity.value = 0;
		curTime= 0;
		isHit = false;
	}
	
	private IEnumerator WarningEffect()
	{
		while (isEffectEnable)
		{
			effectIntensity = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * maxAlpha;
			vignette.intensity.value = effectIntensity;
			
			yield return null;
		}
		
		vignette.intensity.value = 0f;
	}
}
