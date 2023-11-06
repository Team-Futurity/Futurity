using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconMove : MonoBehaviour
{
	public RectTransform icon;

	private Vector2 startPos;
	private Vector2 endPos;

	private bool isActive = false;

	public void MoveIcon(float percent)
	{
		if (percent <= .0f || isActive) return;

		isActive = true;

		Debug.Log(percent);

		var targetPos = Mathf.Lerp(0, 1, percent);

		StartCoroutine("StartMove", targetPos);
	}

	private IEnumerator StartMove(float targetPos)
	{
		while(true)
		{
			yield return new WaitForSeconds(0.1f);
		}
	}
}
