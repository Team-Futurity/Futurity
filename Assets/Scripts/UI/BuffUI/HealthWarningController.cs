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
	private StatusManager statusManager;  // �÷��̾� ü��

	[SerializeField]
	/// <summary>
	/// ������ ȿ�� �̹���
	/// </summary>
	private GameObject redEffectObject;
	private Image redEffectImage;

	[SerializeField]
	/// <summary>
	/// ȿ���� �ִ� ����
	/// </summary>
	private float maxAlpha = 0.25f;

	private float effectIntensity = 0f;

	[SerializeField]
	private float pulseSpeed = 2f;  // ȿ���� �ڵ� �ӵ�

	private void Start()
	{
		// backgroundPanel�� �Ҵ���� �ʾҴٸ� ���ο� GameObject�� �����մϴ�
		if (redEffectObject == null)
		{
			redEffectObject = new GameObject("HealthWarringEffectObject");
			redEffectImage = redEffectObject.AddComponent<Image>();
			redEffectImage.color = Color.clear;

			// �ֻ��� ĵ������ ã�� backgroundPanel�� �� ĵ������ �ڽ����� �����մϴ�
			Canvas topCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			redEffectObject.transform.SetParent(topCanvas.transform, false);

			// backgroundPanel�� ��Ŀ�� ȭ�� ��ü�� �����մϴ�
			RectTransform rectTransform = redEffectObject.GetComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			redEffectObject.SetActive(false);
		}
		else
		{
			redEffectImage = redEffectObject.GetComponent<Image>();
			redEffectObject.SetActive(false);
		}
	}

	/// <summary>
	/// �����Ӹ��� ȿ���� ü�¿� ���� �����ϰ�, �׽�Ʈ������ ü���� ���ҽ�ŵ�ϴ�.
	/// </summary>
	private void Update()
	{
		health = statusManager.GetStatus(StatusType.CURRENT_HP).GetValue();

		// ü���� 60 �����̸� ȿ�� ����
		if (health <= 60)
		{
			redEffectImage.gameObject.SetActive(true);
			PulseEffect();
		}
		else
		{
			redEffectImage.gameObject.SetActive(false);
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
		redEffectImage.color = new Color(1, 0, 0, effectIntensity);
	}
}
