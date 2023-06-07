using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;


/// <summary>
/// ȭ�� ���̵��ΰ� ���̵�ƿ��� �����ϴ� Ŭ�����Դϴ�.
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

			//rectTransform �ʱ�ȭ(ȭ�� ��ü �Ҵ�)
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
	/// ���̵带 �����ϴ� �Լ��Դϴ�. ���ڷ� ���̵���, ���̵�ƿ� ����, ���̵� �ð�, ���̵� ������ �޽��ϴ�.
	/// </summary>
	/// <param name="isFadeInOut">���̵��� ���θ� �����մϴ�. true�� ���̵���, false�� ���̵�ƿ��� �����մϴ�.</param>
	/// <param name="fadeTime">���̵尡 �̷������ ������ �ð��� �����մϴ�.</param>
	/// <param name="fadeColor">���̵��� ������ �����մϴ�.</param>
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
	/// ���̵� �ڷ�ƾ �Լ��Դϴ�.
	/// </summary>
	/// <param name="startAlpha">���̵尡 ���۵� ���� ���İ��� �����մϴ�.</param>
	/// <param name="endAlpha">���̵尡 ���� ���� ���İ��� �����մϴ�.</param>
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