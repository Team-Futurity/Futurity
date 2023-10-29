using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionContoller : MonoBehaviour
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject model;
	[SerializeField] private Animator animator;
	[SerializeField] private UnitBase unit;
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
	}

	// ¾Ö´Ï¸ÞÀÌ¼Ç ÀüÈ¯ ½Ã ½ÇÇà
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
			Vector3 currentPosition = parent.transform.position;
			Vector3 nextPosition = parent.transform.position + deltaPosition;
			Vector3 direction = deltaPosition.normalized;
			float predictedDistancePerFrame = deltaPosition.magnitude;

			Vector3 predictedPosition = nextPosition + direction * predictedDistancePerFrame + Vector3.up * 0.5f;

			unit.DisableAllCollider();
			if(!Physics.Linecast(currentPosition, predictedPosition, out var hit))
			{
				parent.transform.position = nextPosition;
			}
			unit.RestoreCollider();
		}
	}
}
