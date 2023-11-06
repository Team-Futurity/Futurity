using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreditViewer : MonoBehaviour
{
	// Text
	public RectTransform content;
	private float maxPosY = .0f;
	private float startPosY = .0f;
	
	// Image
	public Image bgImage;
	private float bgMaxPosX = .0f;
	private float startPosX = .0f;
	
	// Time
	public float duration = .0f;
	private float timer = .0f;

	private void Start()
	{
		// Text
		startPosY = 0f - (content.rect.height * 0.2f);
		maxPosY = content.rect.height;
		
		// Image
		startPosX = .0f;
		bgMaxPosX = bgImage.rectTransform.rect.width / 2f;
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer > duration)
		{
			SceneLoader.Instance.LoadScene("TitleScene");
			return;
		}

		var textResult = Mathf.Lerp(startPosY, maxPosY, timer / duration);
		var textResultPos = content.anchoredPosition;

		textResultPos.y = textResult;
		content.anchoredPosition = textResultPos;
		
		var imageResult = Mathf.Lerp(startPosX, bgMaxPosX, timer / duration);
		var imageResultPos = bgImage.rectTransform.anchoredPosition;

		imageResultPos.x = -imageResult;
		bgImage.rectTransform.anchoredPosition = imageResultPos;
	}
}
