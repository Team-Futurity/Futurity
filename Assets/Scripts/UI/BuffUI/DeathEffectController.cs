using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeathEffectController : MonoBehaviour
{
	[Header("필수 변수")]
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private Canvas panelCanvas;
	[Space(15)]

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
	[Space(15)]

	[Header("Debug용")]
	[SerializeField]
	private GameObject backgroundPanel;


	[SerializeField]
	private UnityEvent DeathEndEvent;

	private Camera cameraComponent;
	private Image backgroundImage;

	void Start()
	{
		cameraComponent = cameraTransform.GetComponent<Camera>();

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

	public void PlayerDeath()
	{
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

		yield return new WaitForSeconds(1f);
		DeathEndEvent?.Invoke();
	}

	void OnDisable()
	{
		Time.timeScale = 1f;
		backgroundPanel.SetActive(false);
		cameraComponent.orthographicSize = normalCameraSize;
	}
}
