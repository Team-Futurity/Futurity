using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionContoller : MonoBehaviour
{
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
}
