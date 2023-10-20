using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionContoller : MonoBehaviour
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject model;
	[SerializeField] private Animator animator;
	[SerializeField] private List<AnimationType> animations;
	private Dictionary<string, AnimationType> animationDic;
	[SerializeField] private bool currentApplyRootMotion;
	[SerializeField] private float stopDistance;

	private void Awake()
	{
		animationDic = new Dictionary<string, AnimationType>();
		foreach(var anim in animations)
		{
			animationDic.Add(anim.animationName, anim);
		}

		//StartCoroutine(RefreshRootMotionCoroutine());
	}

	// 애니메이션 전환 시 실행
	public bool SetRootMotion(string animName, int layer = 0)
	{
		if(animName == null) { return false; }

		AnimationType type = null;
		if(!animationDic.TryGetValue(animName, out type)) { return false; }

		animator.applyRootMotion = type.isRootMotion;
		currentApplyRootMotion = type.isRootMotion;
		
		return true;
	}

	public void SetStopDistance(float distance)
	{
		stopDistance = distance;
	}

	private void OnAnimatorMove()
	{
		if(animator == null) { return; }

		if (currentApplyRootMotion)
		{
			Vector3 deltaPosition = animator.deltaPosition;
			Vector3 movedPosition = parent.transform.position + deltaPosition; // 최종적으로 위치할 것으로 예상되는 위치
			FDebug.DrawRay(parent.transform.position, animator.deltaPosition.normalized * stopDistance, Color.red);
			deltaPosition.y = 0;

			// 애니메이션 방향으로 Stop Distance까지 Ray 사용 
			if (Physics.Raycast(parent.transform.position, animator.deltaPosition.normalized, out RaycastHit hit, stopDistance))
			{
				// Unit(Enemy, Player)의 레이어일 경우에만 동작
				if (hit.transform.gameObject.tag == "Enemy")
				{
					FDebug.Log("Distance_" + hit.distance + ", Direction_" + -parent.transform.forward);

					// 너무 과도하게 붙은 경우 일정 거리 앞에서 멈추도록 하는 코드
					movedPosition = hit.transform.position - parent.transform.forward * stopDistance;
					movedPosition.y = 0;

					FDebug.Log("M" + movedPosition);
					parent.transform.position = movedPosition;
				}
			}
			else
			{
				// 변화값만큼 이동
				FDebug.Log("D" + deltaPosition);
				parent.transform.position += deltaPosition;
				parent.transform.rotation *= animator.deltaRotation;
			}
		}
		/*Vector3 vec = parent.transform.position;
		vec.y = 0;
		parent.transform.position = vec;*/
	}

	private IEnumerator RefreshRootMotionCoroutine()
	{
		while (true)
		{
			Vector3 vector = model.transform.position;
			/*string currentAnimName = GetName();

			if (currentAnimName == "") { FDebug.LogError("Animation Name is not Matched", GetType()); yield return null; continue; }

			FDebug.Log(transform.position + "_" + (transform.position + transform.forward * 0.3f));

			if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, 0.3f))
			{
				FDebug.Log(vector + "__" + (vector - transform.forward * (hit.distance - 0.2f)));

				vector -= transform.forward * (hit.distance - 0.2f);
			}

			transform.position += new Vector3(vector.x * animationDic[currentAnimName].applyX, vector.y * animationDic[currentAnimName].applyY, vector.z * animationDic[currentAnimName].applyZ);*/
			/*transform.position = vector;
			model.transform.localPosition = Vector3.zero;*/

			yield return null;
		}
	}

	private string GetName()
	{
		string animationName = "";
		foreach(var animName in animationDic.Keys)
		{
			if(animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
			{
				animationName = animName;
				break;
			}
		}

		return animationName;
	}
}
