using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthWarningController : MonoBehaviour
{
	[SerializeField]
	private float health = 100.0f;  // �÷��̾� ü��
	
	[Header("�ʼ� ����")]
	[SerializeField] [Tooltip("�÷��̾� ����")] private StatusManager statusManager;
	
	[Header("���� �ڵ� ����Ʈ")]
	[SerializeField] private Volume volume;
	[SerializeField] private float maxAlpha = 0.75f;
	[SerializeField] private float pulseSpeed = 2f;  // ȿ���� �ڵ� �ӵ�
	
	private float effectIntensity = 0f;
	private EnemyManager enemyManager;
	private Vignette vignette;
	private bool isDeath = false;
	
	private void Start()
	{
		Camera.main.gameObject.TryGetComponent<Volume>(out volume);
		enemyManager = EnemyManager.Instance;

		if (volume.profile.TryGet<Vignette>(out vignette))
		{
		}
		
		isDeath = false;
	}
	
	private void Update()
	{
		health = statusManager.GetStatus(StatusType.CURRENT_HP).GetValue();

		if(!isDeath && health <= 0)
		{
			PulseEffect(false);
			PlayerDeath();
		}

		// ü���� 60 �����̸� ��� ���� ����
		if (health <= 60)
		{
			PulseEffect(true);
		}
		else
		{
			PulseEffect(false);
		}
	}
	
	private void PulseEffect(bool isActive)
	{
		if (isActive)
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
