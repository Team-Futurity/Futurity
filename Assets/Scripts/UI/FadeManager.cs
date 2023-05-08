using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

[Singleton(Automatic = true, Persistent = true, Name = "FadeManager", HideFlags = HideFlags.None)]
public class FadeManager : MonoBehaviour, ISingleton
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Color _fadeColor = Color.black;
	[SerializeField]
	private float _fadeTime = 1.0f;
	[SerializeField]
	private Image _fadeImage;

	private Coroutine _currentFadeCoroutine;


	private void Awake()
	{
		Singleton<FadeManager>.Awake(this);

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


	//isFadeInOut이 true라면 FadeIn 아니라면 FadeOut
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