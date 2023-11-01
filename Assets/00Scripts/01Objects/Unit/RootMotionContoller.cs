using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColliderLayerData
{
	public GameObject collider;
	public int layer;
}

public class RootMotionContoller : MonoBehaviour
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject model;
	[SerializeField] private Animator animator;
	[SerializeField] private ColliderLayerData[] colliders;
	[SerializeField] private List<AnimationType> animations;
	private Dictionary<string, AnimationType> animationDic;
	[SerializeField] private bool currentApplyRootMotion;
	[SerializeField] private float stopDistance;
	private LayerMask ignoreLayerMask;
	private int ignoreRay = LayerMask.NameToLayer("Ignore Raycast");

	private void Awake()
	{
		animationDic = new Dictionary<string, AnimationType>();
		foreach(var anim in animations)
		{
			animationDic.Add(anim.animationName, anim);
		}

		ignoreLayerMask = -1 & ~ignoreRay;
	}

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

	private void SetIgnoreLayer()
	{
		for(int i = 0; i <  colliders.Length; i++)
		{
			colliders[i].collider.layer = ignoreRay;
		}
	}

	private void SetLayer()
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].collider.layer = colliders[i].layer;
		}
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

			SetIgnoreLayer();
			if (!Physics.Linecast(currentPosition, predictedPosition, out var hit, ignoreLayerMask))
			{
				parent.transform.position = nextPosition;
			}
			SetLayer();
		}
	}
}
