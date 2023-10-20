using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TraceObject : MonoBehaviour
{
	[SerializeField] private GameObject targetObject;
	[SerializeField] private float moveDelay;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float timeToReachMaximumSpeed;
	[SerializeField] private float allowingDistance;
	[SerializeField] private AnimationCurve speedCurve;

	private float currentTime;
	private bool isMoveDelayTime; // move Delay�� ���۵Ǿ����� ����
	private bool isMoveStart; // move�� ���۵Ǿ����� ����
	private bool isMoveEnd; // move End ��ȣ�� ���Դ��� ����
	private WaitForSeconds delayPreMoveWFS;

	private void Start()
	{
		isMoveStart = false;
		isMoveDelayTime = false;
		isMoveEnd = false;

		StartCoroutine(MoveCoroutine());
	}

	public void OnDelayPreMove()
	{
		isMoveDelayTime = true;
	}

	public void OnStop()
	{
		isMoveEnd = true;
	}

	private IEnumerator MoveCoroutine()
	{
		currentTime = 0;
		delayPreMoveWFS = new WaitForSeconds(moveDelay);
		float speed = 0;
		while (true)
		{
			if(isMoveDelayTime)
			{
				yield return delayPreMoveWFS;
				isMoveDelayTime = false;
				isMoveStart = true;
			}

			if(isMoveStart)
			{
				float normalizedTime;
				currentTime += Time.deltaTime;
				normalizedTime = currentTime / timeToReachMaximumSpeed;
				speed = moveSpeed * speedCurve.Evaluate(normalizedTime);

				TraceToTarget(speed);

				if(isMoveEnd)
				{
					if (Vector3.Distance(transform.position, targetObject.transform.position) <= allowingDistance)
					{
						isMoveStart = false;
						isMoveDelayTime = false;
					}
				}
			}

			yield return null;
		}
	}

	private void TraceToTarget(float speed)
	{
		Vector3 direction = targetObject.transform.position - transform.position;

		transform.position += direction * speed * Time.deltaTime;
	}
}
