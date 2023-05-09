using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
	[SerializeField] private Transform targetPosition;
	[SerializeField] private float fadeDuration = 0.5f;
	[SerializeField] private float warpEndDelay = 0.1f;
	private WaitForSeconds waitForFadeDuration;

	private void Start()
	{
		waitForFadeDuration = new WaitForSeconds(fadeDuration + warpEndDelay);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(FadeAndWarpCoroutine(collision.gameObject));
		}
	}

	private IEnumerator FadeAndWarpCoroutine(GameObject player)
	{
		FadeManager.Instance.FadeStart(false, fadeDuration, Color.black);
		yield return waitForFadeDuration;
		player.transform.position = targetPosition.position;
		FadeManager.Instance.FadeStart(true, fadeDuration, Color.black);
	}
}
