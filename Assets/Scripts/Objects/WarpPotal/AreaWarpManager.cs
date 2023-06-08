using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaWarpManager : Singleton<AreaWarpManager>
{
	private Transform targetPosition;
	private GameObject useObject;

	[SerializeField]
	/// <summary>
	/// ���̵��� ���� �ð��� �����մϴ�.
	/// </summary>
	private float fadeDelay = 0.5f;
	[SerializeField]
	/// <summary>
	/// ���� ���� ���� ���� �ð��� �����մϴ�.
	/// </summary>
	private float warpEndDelay = 0.1f;
	private WaitForSeconds waitForFadeDuration;

	private void Start()
	{
		waitForFadeDuration = new WaitForSeconds(fadeDelay + warpEndDelay);
	}

	/// <summary>
	/// ������ �����ϴ� �Լ��Դϴ�. ������ ����ϴ� ��ü�� ��� ��ġ, �׸��� ���� �ð��� �Ű������� �޽��ϴ�.
	/// </summary>
	/// <param name="useObject">������ ����ϴ� ��ü�Դϴ�.</param>
	/// <param name="targetPosition">������ ��� ��ġ�Դϴ�.</param>
	/// <param name="delay">���̵� ���� �ð��Դϴ�.</param>
	public void WarpStart(GameObject useObject, Transform targetPosition, float delay, UnityEvent warpEndEvent)
	{
		this.useObject = useObject;
		this.targetPosition = targetPosition;
		fadeDelay = delay;

		StartCoroutine(FadeAndWarpCoroutine(warpEndEvent));
	}

	/// <summary>
	/// ���̵�� ������ ���ÿ� �����ϴ� �ڷ�ƾ�Դϴ�.
	/// </summary>
	private IEnumerator FadeAndWarpCoroutine(UnityEvent warpEndEvent)
	{
		FadeManager.Instance.FadeStart(false, fadeDelay, Color.black);
		yield return waitForFadeDuration;
		useObject.transform.position = targetPosition.position;
		yield return FadeManager.Instance.FadeCoroutineStart(true, fadeDelay, Color.black);
		warpEndEvent?.Invoke();
	}
}
