using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// PlayerHealthUI Ŭ������ �÷��̾� ü�¿� ���� ȭ�� ȿ���� �����մϴ�.
/// </summary>
public class HealthWarningController : MonoBehaviour
{
	[SerializeField]
	private float health = 100.0f;  // �÷��̾� ü��


	[Header("�ʼ� ����")]
	[SerializeField]
	[Tooltip("panel�� ������ canvas")]
	private Canvas panelCanvas;
	[SerializeField]
	[Tooltip("�÷��̾� ����")]
	private StatusManager statusManager;
	[SerializeField]
	[Tooltip("�÷��̾� ����� ȣ���� Window")]
	private GameObject deathOpenWindow;

	[Space(15)]
	[Header("�׽�Ʈ�� �Ҵ����ּ���")]
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Volume volume;
	private Vignette vignette; 
	
	[Header("Death ���� ���� ����")]
	[SerializeField]
	private float zoomInSpeed = 2f;
	[SerializeField]
	private float fadeInSpeed = 2f;
	[SerializeField]
	private float fadeInAlphaMax = 1f;
	[SerializeField]
	private float slowMotionFactor = 0.2f;
	[SerializeField]
	private float normalCameraSize = 5f;
	[SerializeField]
	private float zoomInCameraSize = 2f;
	[SerializeField]
	private bool isDeath = false;
	[Space(15)]

	[Header("Debug��")]
	[SerializeField]
	private GameObject backgroundPanel;

	[Space(15)]
	[SerializeField]
	private UnityEvent DeathEndEvent;

	private Camera cameraComponent;
	private Image backgroundImage;
	private WindowManager windowManager;

	[SerializeField]
	/// <summary>
	/// ȿ���� �ִ� ����
	/// </summary>
	private float maxAlpha = 0.75f;

	private float effectIntensity = 0f;

	[SerializeField]
	private float pulseSpeed = 2f;  // ȿ���� �ڵ� �ӵ�

	private void Start()
	{
		Camera.main.gameObject.TryGetComponent<Volume>(out volume);
		cameraTransform = Camera.main.gameObject.transform;
		cameraComponent = cameraTransform.GetComponent<Camera>();
		windowManager = WindowManager.Instance;

		if (volume.profile.TryGet<Vignette>(out vignette)) { }
		isDeath = false;
		if (playerTransform == null)
			playerTransform = transform;


		// backgroundPanel�� �Ҵ���� �ʾҴٸ� ���ο� GameObject�� �����մϴ�
		if (backgroundPanel == null)
		{
			backgroundPanel = new GameObject("BackgroundPanel");
			backgroundImage = backgroundPanel.AddComponent<Image>();
			backgroundImage.color = Color.clear;

			// �ֻ��� ĵ������ ã�� backgroundPanel�� �� ĵ������ �ڽ����� �����մϴ�

			if (panelCanvas == null)
			{
				panelCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			}
			backgroundPanel.transform.SetParent(panelCanvas.transform, false);

			// backgroundPanel�� ��Ŀ�� ȭ�� ��ü�� �����մϴ�
			RectTransform rectTransform = backgroundPanel.GetComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			backgroundPanel.SetActive(false);
		}
		else
		{
			backgroundImage = backgroundPanel.GetComponent<Image>();
			backgroundPanel.SetActive(false);
		}
	}

	/// <summary>
	/// �����Ӹ��� ȿ���� ü�¿� ���� �����ϰ�, �׽�Ʈ������ ü���� ���ҽ�ŵ�ϴ�.
	/// </summary>
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
	
	/// <summary>
	/// ȿ���� ���� �ڵ�ó�� �����ϴ� �޼����Դϴ�.
	/// </summary>
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


	public void PlayerDeath()
	{
		isDeath = true;
		backgroundPanel.SetActive(true);
		backgroundImage.color = Color.clear;
		StartCoroutine(ZoomInAndSlowMotion());
	}

	IEnumerator ZoomInAndSlowMotion()
	{
		Time.timeScale = slowMotionFactor;

		while (cameraComponent.orthographicSize > zoomInCameraSize || backgroundImage.color.a < 0.5f)
		{
			// ī�޶� �� ��
			if (cameraComponent.orthographicSize > zoomInCameraSize)
			{
				cameraComponent.orthographicSize -= zoomInSpeed * Time.deltaTime;
			}

			// ���İ� ����
			if (backgroundImage.color.a < 1f)
			{
				Color newColor = backgroundImage.color;
				newColor = new Color(newColor.r, newColor.g, newColor.b, newColor.a + (Time.deltaTime * fadeInSpeed));
				backgroundImage.color = newColor;
			}

			yield return null;
		}

		Time.timeScale = 1f;
		yield return new WaitForSeconds(0.5f);

		windowManager.WindowOpen(deathOpenWindow, panelCanvas.transform, false, Vector2.zero, Vector2.one);
		DeathEndEvent?.Invoke();
	}

	void OnDisable()
	{
		Time.timeScale = 1f;
		if (backgroundPanel != null)
		{
			backgroundPanel.SetActive(false);
		}
		cameraComponent.orthographicSize = normalCameraSize;
	}
}
