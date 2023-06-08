using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerHealthUI Ŭ������ �÷��̾� ü�¿� ���� ȭ�� ȿ���� �����մϴ�.
/// </summary>
public class HealthWarningController : MonoBehaviour
{
	[SerializeField]
	private float health = 100.0f;  // �÷��̾� ü��

	[SerializeField]
	/// <summary>
	/// ������ ȿ�� �̹���
	/// </summary>
	private Image redEffect;

	[SerializeField]
	/// <summary>
	/// ȿ���� �ִ� ����
	/// </summary>
	private float maxAlpha = 0.25f;

	private float effectIntensity = 0f;
	private float pulseSpeed = 0.5f;  // ȿ���� �ڵ� �ӵ�

	/// <summary>
	/// �����Ӹ��� ȿ���� ü�¿� ���� �����ϰ�, �׽�Ʈ������ ü���� ���ҽ�ŵ�ϴ�.
	/// </summary>
	private void Update()
	{
		// ü���� 60 �����̸� ȿ�� ����
		if (health <= 60)
		{
			redEffect.gameObject.SetActive(true);
			PulseEffect();
		}
		else
		{
			redEffect.gameObject.SetActive(false);
		}

		// �׽�Ʈ�� ���� ü�� ���� �ڵ�
		// ���� ���ӿ����� �÷��̾ �������� ���� �� ü���� ���ҽ�Ű�� �ڵ带 ����մϴ�.
		if (Input.GetKeyDown(KeyCode.Space))
		{
			health -= 10f;
		}
	}

	/// <summary>
	/// ȿ���� ���� �ڵ�ó�� �����ϴ� �޼����Դϴ�.
	/// </summary>
	private void PulseEffect()
	{
		effectIntensity = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * maxAlpha;
		redEffect.color = new Color(1, 0, 0, effectIntensity);
	}
}
