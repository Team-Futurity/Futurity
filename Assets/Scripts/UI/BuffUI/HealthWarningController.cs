using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerHealthUI 클래스는 플레이어 체력에 따라 화면 효과를 제어합니다.
/// </summary>
public class HealthWarningController : MonoBehaviour
{
	[SerializeField]
	private float health = 100.0f;  // 플레이어 체력

	[SerializeField]
	/// <summary>
	/// 적용할 효과 이미지
	/// </summary>
	private Image redEffect;

	[SerializeField]
	/// <summary>
	/// 효과의 최대 투명도
	/// </summary>
	private float maxAlpha = 0.25f;

	private float effectIntensity = 0f;
	private float pulseSpeed = 0.5f;  // 효과의 박동 속도

	/// <summary>
	/// 프레임마다 효과를 체력에 따라 조정하고, 테스트용으로 체력을 감소시킵니다.
	/// </summary>
	private void Update()
	{
		// 체력이 60 이하이면 효과 적용
		if (health <= 60)
		{
			redEffect.gameObject.SetActive(true);
			PulseEffect();
		}
		else
		{
			redEffect.gameObject.SetActive(false);
		}

		// 테스트를 위한 체력 감소 코드
		// 실제 게임에서는 플레이어가 데미지를 받을 때 체력을 감소시키는 코드를 사용합니다.
		if (Input.GetKeyDown(KeyCode.Space))
		{
			health -= 10f;
		}
	}

	/// <summary>
	/// 효과를 심장 박동처럼 조절하는 메서드입니다.
	/// </summary>
	private void PulseEffect()
	{
		effectIntensity = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * maxAlpha;
		redEffect.color = new Color(1, 0, 0, effectIntensity);
	}
}
