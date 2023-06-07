using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;


/// <summary>
/// 화면 페이드인과 페이드아웃을 제어하는 클래스입니다.
/// </summary>
[Singleton(Automatic = true, Persistent = true, Name = "FadeManager", HideFlags = HideFlags.None)]

public class FadeManager : Singleton<FadeManager>
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Color _fadeColor = Color.black;
	[SerializeField]
	private float _fadeTime = 1.0f;
	private Image _fadeImage;

	private Coroutine _currentFadeCoroutine;


	private void Awake()
	{
		base.Awake();

		if (_canvasGroup == null)
		{
			GameObject fadeCanvas = new GameObject("FadeCanvas");
			fadeCanvas.transform.SetParent(transform);

			Canvas canvas = fadeCanvas.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = int.MaxValue;

			_canvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
			_canvasGroup.blocksRaycasts = false;
			_canvasGroup.alpha = 0;

			//rectTransform 초기화(화면 전체 할당)
			RectTransform rectTransform = fadeCanvas.GetComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.anchoredPosition = Vector2.zero;

			if (!_fadeImage)
			{
				_fadeImage = fadeCanvas.AddComponent<Image>();
			}
			_fadeImage.color = _fadeColor;
		}
	}

	/// <summary>
	/// 페이드를 시작하는 함수입니다. 인자로 페이드인, 페이드아웃 여부, 페이드 시간, 페이드 색상을 받습니다.
	/// </summary>
	/// <param name="isFadeInOut">페이드인 여부를 설정합니다. true면 페이드인, false면 페이드아웃을 수행합니다.</param>
	/// <param name="fadeTime">페이드가 이루어지는 동안의 시간을 설정합니다.</param>
	/// <param name="fadeColor">페이드의 색상을 설정합니다.</param>
	public void FadeStart(bool isFadeInOut, float fadeTime, Color fadeColor)
	{
		_fadeTime = fadeTime;
		_fadeColor = fadeColor;
		_fadeImage.color = _fadeColor;

		float startAlpha = 0;
		float endAlpha = 0;
		if (isFadeInOut)
		{
			startAlpha = 1;
		}
		else
		{
			endAlpha = 1;
		}


		if (_currentFadeCoroutine != null)
		{
			StopCoroutine(_currentFadeCoroutine);
		}


		_currentFadeCoroutine = StartCoroutine(FadeCoroutine(startAlpha, endAlpha));
	}


	/// <summary>
	/// 페이드 코루틴 함수입니다.
	/// </summary>
	/// <param name="startAlpha">페이드가 시작될 때의 알파값을 설정합니다.</param>
	/// <param name="endAlpha">페이드가 끝날 때의 알파값을 설정합니다.</param>
	private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
	{
		_canvasGroup.alpha = startAlpha;
		_canvasGroup.blocksRaycasts = true;

		float elapsedTime = 0;

		while (elapsedTime < _fadeTime)
		{
			elapsedTime += Time.deltaTime;
			_canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / _fadeTime);

			yield return null;
		}

		_canvasGroup.alpha = endAlpha;
		_canvasGroup.blocksRaycasts = false;

		_currentFadeCoroutine = null;
	}
}