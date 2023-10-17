using System;
using UnityEngine;

[Serializable]
public class AnimationType
{
	public string animationName;
	public bool isRootMotion;
	[Range(0,1)] public int applyX;
	[Range(0, 1)] public int applyY;
	[Range(0, 1)] public int applyZ;
}
