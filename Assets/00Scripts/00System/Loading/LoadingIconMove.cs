using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconMove : MonoBehaviour
{
	private RectTransform rectTransform;
	private Vector2 startPos;
	private Vector2 endPos;

	private float screenMaxWidth = .0f;

	private float moveDistance = .0f;

	private float timer = .0f;

	private bool isActive = false;

	private void Awake()
	{
		TryGetComponent(out rectTransform);

		screenMaxWidth = Screen.width;

		moveDistance = screenMaxWidth - Mathf.Abs(rectTransform.anchoredPosition.x);

		startPos = rectTransform.anchoredPosition;
		endPos = new Vector2(
			rectTransform.anchoredPosition.x + (moveDistance),
			rectTransform.anchoredPosition.y
		);
	}

	public void MoveIcon(float percent)
	{
		if (percent <= .0f || isActive) return;

		isActive = true;

		var targetPos = new Vector2(endPos.x * percent, endPos.y);
		StartCoroutine("StartMove", targetPos);
	}

	private IEnumerator StartMove(Vector2 targetPos)
	{
		while (Vector2.Distance(startPos, targetPos) > 0.1f && isActive)
		{
			timer += 0.1f;

			var resultPos = Vector2.Lerp(startPos, targetPos, timer);

			yield return new WaitForSeconds(0.01f);

			rectTransform.anchoredPosition = resultPos;
		}

		startPos = rectTransform.anchoredPosition;
		timer = .0f;
		isActive = false;
	}
}
