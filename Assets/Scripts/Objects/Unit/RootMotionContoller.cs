using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionContoller : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private AnimationClip clip;

	private void Update()
	{
		//FDebug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name + "_" + animator.GetCurrentAnimatorStateInfo(0).);
		//FDebug.Log($"_____{clip.name}_{animator.}");
		
	}
}
