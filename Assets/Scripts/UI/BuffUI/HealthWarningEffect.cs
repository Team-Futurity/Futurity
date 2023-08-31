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
	private bool isEffectEnable = false;
	private EnemyManager enemyManager;
	private Vignette vignette;
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
			
			pulseEffect = PulseEffect();
			StartCoroutine(pulseEffect);
		}
		else if (hp > 60 && isEffectEnable == true)
		{
			vignette.intensity.value = 0f;
			isEffectEnable = false;	
		}
	}

	private IEnumerator PulseEffect()
	{
		while (isEffectEnable)
		{
			effectIntensity = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * maxAlpha;
			vignette.intensity.value = effectIntensity;
			
			yield return null;
		}
	}
	
	private void PlayerDeath()
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

}
