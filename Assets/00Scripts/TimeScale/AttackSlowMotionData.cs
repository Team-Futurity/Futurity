using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowMotion Data", menuName = "ScriptableObject/SlowMotion Data", order = int.MaxValue)]
public class AttackSlowMotionData : ScriptableObject
{
	[Header("최소 타임스케일")] 
	[SerializeField] private float minTimeScale;
	public float MinTimeScale => minTimeScale;

	[Header("최대 타임 스케일")] 
	[SerializeField] private float maxTimeScale;
	public float MaxTimeScale => maxTimeScale;

	[Header("최소 타임스케일 유지 시간")] 
	[SerializeField] private float durationMin;
	public float DurationMin => durationMin;

	[Header("최대 타임스케일 유지 시간")] 
	[SerializeField] private float durationMax;
	public float DurationMax => durationMax;

}
