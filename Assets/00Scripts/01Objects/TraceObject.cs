using System.Collections;
using UnityEngine;

public class TraceObject : MonoBehaviour
{
	[SerializeField] private GameObject targetObject;
	[SerializeField] private float moveDelay;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotateSpeed;
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

	private void FixedUpdate()
	{
		if (isMoveDelayTime)
		{
			currentTime += Time.fixedDeltaTime;

			Vector3 direction = targetObject.transform.position - transform.position;
			Vector3 rotVec = direction;

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotVec), rotateSpeed * Time.deltaTime);
			
			if(currentTime >= moveDelay)
			{
				isMoveDelayTime = false;
				isMoveStart = true;
				currentTime = 0;
			}
		}


		if (isMoveStart)
		{
			float normalizedTime;
			float speed = 0;
			currentTime += Time.fixedDeltaTime;
			normalizedTime = currentTime / timeToReachMaximumSpeed;
			speed = moveSpeed * speedCurve.Evaluate(normalizedTime);

			TraceToTarget(speed);

			if (isMoveEnd)
			{
				if (Vector3.Distance(transform.position, targetObject.transform.position) <= allowingDistance)
				{
					isMoveStart = false;
					isMoveDelayTime = false;
					currentTime = 0;
				}
			}
		}
	}

	private IEnumerator MoveCoroutine()
	{
		currentTime = 0;
		delayPreMoveWFS = new WaitForSeconds(moveDelay);
		
		while (true)
		{
			

			yield return null;
		}
	}

	private void TraceToTarget(float speed)
	{
		Vector3 direction = targetObject.transform.position - transform.position;
		Vector3 rotVec = direction;

		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotVec), rotateSpeed * Time.deltaTime);
		transform.position += direction * speed * Time.fixedDeltaTime;
	}
}
