using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
	[SerializeField] private GameObject modelPelvis;
	[SerializeField] private Animator animator;
	[SerializeField] private ColliderLayerData[] colliders;
	[SerializeField] private List<AnimationType> animations;
	private Dictionary<string, AnimationType> animationDic;
	[SerializeField] private bool currentApplyRootMotion;
	[SerializeField] private float stopDistance;
	private LayerMask ignoreLayerMask;
	private int ignoreRay;
	private AnimationType currentAnimationType;
	private int floatingCount = 5;
	private int multiplyNumber;

	private void Awake()
	{
		animationDic = new Dictionary<string, AnimationType>();
		foreach(var anim in animations)
		{
			animationDic.Add(anim.animationName, anim);
		}
		ignoreRay = LayerMask.NameToLayer("Ignore Raycast");
		ignoreLayerMask = 1 << ignoreRay;
		multiplyNumber = (int)Mathf.Pow(10, floatingCount);
	}

	public bool SetRootMotion(string animName, int layer = 0)
	{
		if(animName == null) { return false; }

		AnimationType type = null;
		if(!animationDic.TryGetValue(animName, out type)) { return false; }

		animator.applyRootMotion = type.isRootMotion;
		currentApplyRootMotion = type.isRootMotion;

		currentAnimationType = type;

		Vector3 pos = parent.transform.position;
		pos.y = 0;
		parent.transform.position = pos;

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
			FDebug.Log($"{animator.GetCurrentAnimatorClipInfo(0)[0].clip.name} : {deltaPosition}");


			deltaPosition.x = currentAnimationType.isApplyX ? deltaPosition.x : 0f;
			deltaPosition.y = currentAnimationType.isApplyY ? deltaPosition.y : 0f;
			deltaPosition.z = currentAnimationType.isApplyZ ? deltaPosition.z : 0f;

			Vector3 currentPosition = modelPelvis.transform.position;
			Vector3 modelNextPosition = modelPelvis.transform.position + deltaPosition;
			Vector3 nextPosition = parent.transform.position + deltaPosition;
			Vector3 direction = deltaPosition.normalized;
			float predictedDistancePerFrame = deltaPosition.magnitude;

			Vector3 predictedPosition = modelNextPosition + direction * predictedDistancePerFrame + Vector3.up * 0.5f;

			SetIgnoreLayer();
			if (!Physics.Linecast(currentPosition, predictedPosition, out var hit, ~ignoreLayerMask))
			{
				nextPosition.x = math.trunc(nextPosition.x * multiplyNumber) / multiplyNumber;
				nextPosition.y = math.trunc(nextPosition.y * multiplyNumber) / multiplyNumber;
				nextPosition.z = math.trunc(nextPosition.z * multiplyNumber) / multiplyNumber;

				parent.transform.position = nextPosition;
			}
			else
			{
				FDebug.Log("Linecast : " + hit.collider.ToString());
			}
			SetLayer();
		}
	}
}
