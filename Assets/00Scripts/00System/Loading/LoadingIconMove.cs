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

		// Max Move Distance
		moveDistance = screenMaxWidth - Mathf.Abs(rectTransform.anchoredPosition.x);
		startPos = rectTransform.anchoredPosition;
		endPos = new Vector2(
			rectTransform.anchoredPosition.x + (moveDistance),
			rectTransform.anchoredPosition.y
		);

		// 0 ~ 0.4 : Point 
		// 0.1 , 0.23, 0.28, 0.34, 0.4 
	}

	// 0 ~ 0.4 : 그냥 간다
	// 0.4 ~ 1 : Scene Progress의 영향을 받는다.
	public void MoveIcon(float percent)
	{
		var targetPos = new Vector2(endPos.x * percent, endPos.y);
		StartCoroutine("StartMove", targetPos);
	}

	private IEnumerator StartMove(Vector2 targetPos)
	{
		while (timer <= 1f && Vector2.Distance(startPos, endPos) > 0.1f)
		{
			timer += Time.deltaTime * 0.1f;

			var resultPos = Vector2.Lerp(startPos, targetPos, timer);

			yield return new WaitForSeconds(0.1f);

			rectTransform.anchoredPosition = resultPos;
		}

		startPos = rectTransform.anchoredPosition;
		timer = .0f;
	}
}
