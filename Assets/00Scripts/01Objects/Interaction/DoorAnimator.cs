using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private SkeletonAnimation doorAnimation;
	[SerializeField] private Collider doorCollider;

	[Header("딜레이 타임")] 
	[SerializeField] private float delayTime = 1.0f;

	private IEnumerator doorOpen;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}

		StartCoroutine(DoorAnimation(true));
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		StartCoroutine(DoorAnimation(false));
	}
	
	private IEnumerator DoorAnimation(bool isOpen)
	{
		if (isOpen == true)
		{
			doorAnimation.AnimationState.SetAnimation(0, "open", false);
		}
		else
		{
			doorAnimation.AnimationState.SetAnimation(0, "close", false);
		}

		yield return new WaitForSeconds(delayTime);

		doorCollider.enabled = !isOpen;
	}
}
