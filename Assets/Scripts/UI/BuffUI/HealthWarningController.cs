using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthWarningController : MonoBehaviour
{
	[Header("필수 변수")]
	[SerializeField] [Tooltip("플레이어 스텟")] private StatusManager statusManager;
	
	[Header("심장 박동 이펙트")]
	[SerializeField] private Volume volume;
	[SerializeField] private float maxAlpha = 0.75f;
	[SerializeField] private float pulseSpeed = 2f;  // 효과의 박동 속도
	
	private float effectIntensity = 0f;
	private float playerHp;
	private EnemyManager enemyManager;
	private Vignette vignette;
	private bool isDeath = false;
	
	private void Start()
	{
		enemyManager = EnemyManager.Instance;
		playerHp = statusManager.GetStatus(StatusType.CURRENT_HP).GetValue();
		
		volume.profile.TryGet<Vignette>(out vignette);
		
		isDeath = false;
	}
	
	private void Update()
	{
		// playerHp = statusManager.GetStatus(StatusType.CURRENT_HP).GetValue();
		//
		// if(!isDeath && playerHp <= 0)
		// {
		// 	vignette.intensity.value = 0f;
		// 	PlayerDeath();
		// }
		//
		// if (isDeath == true)
		// {
		// 	return;
		// }
		//
		// PulseEffect();
	}
	
	private void PulseEffect()
	{
		if (playerHp <= 60)
		{
			effectIntensity = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * maxAlpha;
			vignette.intensity.value = effectIntensity;
		}
		else
		{
			vignette.intensity.value = 0f;
		}
	}
	
	private void PlayerDeath()
	{
		isDeath = true;

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
