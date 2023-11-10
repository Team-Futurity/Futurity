using System.Collections;
using UnityEngine;

public class TraceObject : MonoBehaviour
{
	private const string MoveAnimKey = "isMove";

	[SerializeField] private GameObject targetObject;
	[SerializeField] private float moveDelay;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotateSpeed;
	[SerializeField] private float timeToReachMaximumSpeed;
	[SerializeField] private float allowingDistance;
	[SerializeField] private AnimationCurve speedCurve;
	[SerializeField] private Animator animator;

	private float currentTime;
	private bool isMoveDelayTime; // move Delay가 시작되었는지 여부
	private bool isMoveStart; // move가 시작되었는지 여부
	private bool isMoveEnd; // move End 신호가 들어왔는지 여부

	private void Start()
	{
		isMoveStart = false;
		isMoveDelayTime = false;
		isMoveEnd = false;
	}

	public void OnDelayPreMove()
	{
		isMoveDelayTime = true;
	}

	public void OnStop()
	{
		isMoveEnd = true;
	}

	public void SetTargetTransform()
	{
		transform.position = targetObject.transform.position;

		Vector3 direction = targetObject.transform.forward; //targetObject.transform.position - transform.position;

		if(direction != Vector3.zero)
		{
			transform.LookAt(direction);
			transform.rotation = Quaternion.LookRotation(direction);
		}
	}

	private void FixedUpdate()
	{
		if (isMoveDelayTime)
		{
			currentTime += Time.fixedDeltaTime;

			RotateToTarget();

			if (currentTime >= moveDelay)
			{
				isMoveDelayTime = false;
				isMoveStart = true;
				currentTime = 0;
				animator.SetBool(MoveAnimKey, true);
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
					SetTargetTransform();

					isMoveStart = false;
					isMoveDelayTime = false;
					currentTime = 0;
					animator.SetBool(MoveAnimKey, false);
				}
			}
		}
	}

	private void RotateToTarget()
	{
		Vector3 direction = targetObject.transform.forward;

		if(direction == Vector3.zero) { return; }

		Quaternion targetRot = Quaternion.LookRotation(direction);

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
	}

	private void TraceToTarget(float speed)
	{
		RotateToTarget();
		transform.position = Vector3.Slerp(transform.position, targetObject.transform.position, speed * Time.deltaTime); //direction * speed * Time.fixedDeltaTime;
	}
}
