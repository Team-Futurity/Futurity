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

	private void Awake()
	{
		animationDic = new Dictionary<string, AnimationType>();
		foreach(var anim in animations)
		{
			animationDic.Add(anim.animationName, anim);
		}

		StartCoroutine(RefreshRootMotionCoroutine());
	}

	// 애니메이션 전환 시 실행
	public bool SetRootMotion(string animName, int layer = 0)
	{
		if(animName == null) { return false; }

		AnimationType type = null;
		if(!animationDic.TryGetValue(animName, out type)) { return false; }

		animator.applyRootMotion = type.isRootMotion;
		
		return true;
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
			transform.position += vector;
			model.transform.localPosition = Vector3.zero;

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
