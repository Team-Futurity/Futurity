using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWarpManager : Singleton<AreaWarpManager>
{
	private Transform targetPosition;
	private GameObject useObject;

	[SerializeField]
	/// <summary>
	/// 페이드의 지연 시간을 설정합니다.
	/// </summary>
	private float fadeDelay = 0.5f;
	[SerializeField]
	/// <summary>
	/// 워프 종료 후의 지연 시간을 설정합니다.
	/// </summary>
	private float warpEndDelay = 0.1f;
	private WaitForSeconds waitForFadeDuration;

	private void Start()
	{
		waitForFadeDuration = new WaitForSeconds(fadeDelay + warpEndDelay);
	}

	/// <summary>
	/// 워프를 시작하는 함수입니다. 워프를 사용하는 객체와 대상 위치, 그리고 지연 시간을 매개변수로 받습니다.
	/// </summary>
	/// <param name="useObject">워프를 사용하는 객체입니다.</param>
	/// <param name="targetPosition">워프의 대상 위치입니다.</param>
	/// <param name="delay">페이드 지연 시간입니다.</param>
	public void WarpStart(GameObject useObject, Transform targetPosition, float delay)
	{
		this.useObject = useObject;
		this.targetPosition = targetPosition;
		fadeDelay = delay;

		StartCoroutine(FadeAndWarpCoroutine());
	}

	/// <summary>
	/// 페이드와 워프를 동시에 수행하는 코루틴입니다.
	/// </summary>
	private IEnumerator FadeAndWarpCoroutine()
	{
		FadeManager.Instance.FadeStart(false, fadeDelay, Color.black);
		yield return waitForFadeDuration;
		useObject.transform.position = targetPosition.position;
		FadeManager.Instance.FadeStart(true, fadeDelay, Color.black);
	}
}
