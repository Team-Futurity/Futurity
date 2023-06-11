using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// PlayerHealthUI 클래스는 플레이어 체력에 따라 화면 효과를 제어합니다.
/// </summary>
public class HealthWarningController : MonoBehaviour
{
	[SerializeField]
	private float health = 100.0f;  // 플레이어 체력


	[Header("필수 변수")]
	[SerializeField]
	[Tooltip("panel을 생성할 canvas")]
	private Canvas panelCanvas;
	[SerializeField]
	[Tooltip("플레이어 스텟")]
	private StatusManager statusManager;
	[SerializeField]
	[Tooltip("플레이어 사망시 호출할 Window")]
	private GameObject deathOpenWindow;

	[Space(15)]
	[Header("테스트시 할당해주세요")]
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Volume volume;
	private Vignette vignette; 
	
	[Header("Death 연출 관련 변수")]
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

	[Header("Debug용")]
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
	/// 효과의 최대 투명도
	/// </summary>
	private float maxAlpha = 0.75f;

	private float effectIntensity = 0f;

	[SerializeField]
	private float pulseSpeed = 2f;  // 효과의 박동 속도

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


		// backgroundPanel이 할당되지 않았다면 새로운 GameObject를 생성합니다
		if (backgroundPanel == null)
		{
			backgroundPanel = new GameObject("BackgroundPanel");
			backgroundImage = backgroundPanel.AddComponent<Image>();
			backgroundImage.color = Color.clear;

			// 최상위 캔버스를 찾아 backgroundPanel을 그 캔버스의 자식으로 설정합니다

			if (panelCanvas == null)
			{
				panelCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			}
			backgroundPanel.transform.SetParent(panelCanvas.transform, false);

			// backgroundPanel의 앵커를 화면 전체로 설정합니다
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
	/// 프레임마다 효과를 체력에 따라 조정하고, 테스트용으로 체력을 감소시킵니다.
	/// </summary>
	private void Update()
	{
		health = statusManager.GetStatus(StatusType.CURRENT_HP).GetValue();

		if(!isDeath && health <= 0)
		{
			PulseEffect(false);
			PlayerDeath();
		}

		// 체력이 60 이하이면 경고 연출 적용
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
	/// 효과를 심장 박동처럼 조절하는 메서드입니다.
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
			// 카메라 줌 인
			if (cameraComponent.orthographicSize > zoomInCameraSize)
			{
				cameraComponent.orthographicSize -= zoomInSpeed * Time.deltaTime;
			}

			// 알파값 증가
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
