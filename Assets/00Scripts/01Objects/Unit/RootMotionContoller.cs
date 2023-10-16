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
			/*if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, 0.4f))
			{
				vector -= transform.forward * (hit.distance - 0.2f);
			}*/

			transform.position = vector;
			model.transform.localPosition = Vector3.zero;

			yield return null;
		}
	}
}
