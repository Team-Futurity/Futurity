using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWarpManager : Singleton<AreaWarpManager>
{
	private Transform targetPosition;
	private GameObject useObject;

	[SerializeField]
	private float fadeDelay = 0.5f;
	[SerializeField]
	private float warpEndDelay = 0.1f;
	private WaitForSeconds waitForFadeDuration;

	private void Start()
	{
		waitForFadeDuration = new WaitForSeconds(fadeDelay + warpEndDelay);
	}

	public void WarpStart(GameObject useObject, Transform targetPosition, float delay)
	{
		this.useObject = useObject;
		this.targetPosition = targetPosition;
		fadeDelay = delay;

		StartCoroutine(FadeAndWarpCoroutine());
	}

	private IEnumerator FadeAndWarpCoroutine()
	{
		FadeManager.Instance.FadeStart(false, fadeDelay, Color.black);
		yield return waitForFadeDuration;
		useObject.transform.position = targetPosition.position;
		FadeManager.Instance.FadeStart(true, fadeDelay, Color.black);
	}
}
